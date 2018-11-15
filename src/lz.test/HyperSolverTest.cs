// Authors:
//  Roberto Alonso Gómez  <bob@lynza.com>
//
// Copyright (C) 2018 Lynza (http://lynza.com)
//

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using lz.math.solver;
using System.Diagnostics;

namespace lz.test
{
    public struct Gen
    {
        public ulong v;
        public ulong x;
        public ulong y;
    }

    [TestClass]
    public class HyperSolverTest
    {
        public List<Gen> CkVal;
        public HyperSolver Hs;
        public HyperSolverTest()
        {
            Hs = new HyperSolver { B = 7, D = 5, E = 13 };
            CkVal = new List<Gen>();
            Random rnd = new Random();
            for (int i = 0; i < 150; i++)
            {
                ulong x = (ulong)rnd.Next(3, 2000000);
                ulong y = (ulong)rnd.Next(3, 2000000);
                CkVal.Add(new Gen { x = x, y = y, v = Hs.Eval(x, y) });
            }
        }


        [TestMethod]
        public void TestHyperSolverV1()
        {
            Stopwatch watch;
            foreach (Gen g in CkVal)
            {
                watch = Stopwatch.StartNew();
                Result res = Hs.EquSolverV1(g.v);
                watch.Stop();
                Trace.WriteLine($"{new TimeSpan(watch.ElapsedTicks)}\t{g.v}\t\t[{g.x}, {g.y}] [{res.x}, {res.y}]\t{Hs.Eval(res.x, res.y) == g.v}");
            }
        }

        [TestMethod]
        public void TestHyperSolverV2()
        {
            Stopwatch watch;
            foreach (Gen g in CkVal)
            {
                watch = Stopwatch.StartNew();
                Result res = Hs.EquSolverV2(g.v);
                watch.Stop();
                Trace.WriteLine($"{new TimeSpan(watch.ElapsedTicks)}\t{g.v}\t\t[{g.x}, {g.y}] [{res.x}, {res.y}]\t{Hs.Eval(res.x, res.y) == g.v}");
            }
        }

        [TestMethod]
        public void TestHyperSolverV3()
        {
            Stopwatch watch;
            foreach (Gen g in CkVal)
            {
                watch = Stopwatch.StartNew();
                Result res = Hs.EquSolverV3(g.v);
                watch.Stop();
                Trace.WriteLine($"{new TimeSpan(watch.ElapsedTicks)}\t{g.v}\t\t[{g.x}, {g.y}] [{res.x}, {res.y}]\t{Hs.Eval(res.x, res.y) == g.v}");
            }
        }

        [TestMethod]
        public void TestHyperSolverV4()
        {
            Stopwatch watch;
            foreach (Gen g in CkVal)
            {
                watch = Stopwatch.StartNew();
                Result res = Hs.EquSolverV4(g.v);
                watch.Stop();
                Trace.WriteLine($"{new TimeSpan(watch.ElapsedTicks)}\t{g.v}\t\t[{g.x}, {g.y}] [{res.x}, {res.y}]\t{Hs.Eval(res.x, res.y) == g.v}");
            }
        }


        [TestMethod]
        public void TestHyperSolverV5()
        {
            Stopwatch watch;
            foreach (Gen g in CkVal)
            {
                watch = Stopwatch.StartNew();
                Result res = Hs.EquSolverV5(g.v);
                watch.Stop();
                Trace.WriteLine($"{new TimeSpan(watch.ElapsedTicks)}\t{g.v}\t\t[{g.x}, {g.y}] [{res.x}, {res.y}]\t{Hs.Eval(res.x, res.y) == g.v}");
            }
        }
    }
}
