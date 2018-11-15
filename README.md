# HyperSolver

The purpose of this get the first positive solution on solve the Diophantine Equation 

     Bxy + Dx + Ey = F

I create some variant for calculate the speed of this solution 


For test I generate from the 150 sample of possible values of
 
  7xy+5x+13y=GenValue   
 
            Hs = new HyperSolver { B = 7, D = 5, E = 13 };
            CkVal = new List<Gen>();
            Random rnd = new Random();
            for (int i = 0; i < 150; i++)
            {
                ulong x = (ulong)rnd.Next(3, 2000000);
                ulong y = (ulong)rnd.Next(3, 2000000);
                CkVal.Add(new Gen { x = x, y = y, v = Hs.Eval(x, y) });
            }

Testing is the same for each 

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