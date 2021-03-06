﻿// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.


// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Spreads.Threading.Tasks.Channels {
    public static partial class Channel {
        private abstract class Interactor<T> : TaskCompletionSource<T> {
            protected Interactor() : base(
#if NET451
                TaskCreationOptions.None
#else
                TaskCreationOptions.RunContinuationsAsynchronously
#endif
                ) { }

            internal bool Success(T item) {
                bool transitionedToCompleted = TrySetResult(item);
                if (transitionedToCompleted) {
                    Dispose();
                }
                return transitionedToCompleted;
            }

            internal bool Fail(Exception exception) {
                bool transitionedToCompleted = TrySetException(exception);
                if (transitionedToCompleted) {
                    Dispose();
                }
                return transitionedToCompleted;
            }

            public bool TrySetCanceled(CancellationToken token) {
                bool transitionedToCompleted =
#if NET451
                    TrySetCanceled();
#else
                    base.TrySetCanceled(token);
#endif
                if (transitionedToCompleted) {
                    Dispose();
                }
                return transitionedToCompleted;
            }

            protected virtual void Dispose() { }
        }

        private class Reader<T> : Interactor<T> {
            internal static Reader<T> Create(CancellationToken cancellationToken) =>
                cancellationToken.CanBeCanceled ?
                    new CancelableReader<T>(cancellationToken) :
                    new Reader<T>();
        }

        private class Writer<T> : Interactor<VoidResult> {
            internal T Item { get; private set; }

            internal static Writer<T> Create(CancellationToken cancellationToken, T item) {
                Writer<T> w = cancellationToken.CanBeCanceled ?
                    new CancelableWriter<T>(cancellationToken) :
                    new Writer<T>();
                w.Item = item;
                return w;
            }
        }

        private sealed class CancelableReader<T> : Reader<T> {
            private CancellationToken _token;
            private CancellationTokenRegistration _registration;

            internal CancelableReader(CancellationToken cancellationToken) {
                _token = cancellationToken;
                _registration = cancellationToken.Register(s => {
                    var thisRef = (CancelableReader<T>)s;
                    thisRef.TrySetCanceled(thisRef._token);
                }, this);
            }

            protected override void Dispose() => _registration.Dispose();
        }

        private sealed class CancelableWriter<T> : Writer<T> {
            private CancellationToken _token;
            private CancellationTokenRegistration _registration;

            internal CancelableWriter(CancellationToken cancellationToken) {
                _token = cancellationToken;
                _registration = cancellationToken.Register(s => {
                    var thisRef = (CancelableWriter<T>)s;
                    thisRef.TrySetCanceled(thisRef._token);
                }, this);
            }

            protected override void Dispose() => _registration.Dispose();
        }

    }
}
