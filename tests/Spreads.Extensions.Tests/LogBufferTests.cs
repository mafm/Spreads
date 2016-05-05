﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Spreads.Collections.Persistent;
using Spreads.Storage;

namespace Spreads.Extensions.Tests {
    [TestFixture]
    public class LogBufferTests {
        [Test]
        public void LogBufferTest() {
            var sw = new Stopwatch();

            LogBuffer l1 = new LogBuffer("../LogBufferTest", 100);
            LogBuffer l2 = new LogBuffer("../LogBufferTest", 100);

            var tcs = new TaskCompletionSource<int>();
            var tcs2 = new TaskCompletionSource<int>();
            var count = 100;

            //var bytes = new byte[500];
            //for (int i = 0; i < 500; i++) {
            //    bytes[i] = (byte)(i % 255);
            //}

            sw.Start();


            var cnt = 0;
            l1.OnAppend += (ptr, len)  =>
            {
                var message = new byte[len];
                Marshal.Copy(ptr, message, 0, len);
                //var lng = BitConverter.ToInt64(message, 0);
                //Assert.AreEqual(cnt, lng);
                if (count - 1 == cnt) {
                    tcs.SetResult(cnt);
                }
                cnt++;
            };

            var cnt2 = 0;
            l2.OnAppend += (ptr, len) => {
                var message = new byte[len];
                Marshal.Copy(ptr, message, 0, len);
                //var lng = BitConverter.ToInt64(message, 0);
                //Assert.AreEqual(cnt, lng);
                if (count - 1 == cnt2) {
                    tcs2.SetResult(cnt2);
                }
                cnt2++;
            };


            for (int i = 0; i < count; i++)
            {
                l1.Append((long)i);//BitConverter.GetBytes((long)i))); //
            }

            tcs.Task.Wait();
            tcs2.Task.Wait();
            sw.Stop();

            Console.WriteLine($"Elapsed msec: {sw.ElapsedMilliseconds}");
            l1.Dispose();
            l2.Dispose();
            //Thread.Sleep(100000);
        }


    }
}