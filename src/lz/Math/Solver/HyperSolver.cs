// Authors:
//  Roberto Alonso Gómez  <bob@lynza.com>
//
// Copyright (C) 2018 Lynza (http://lynza.com)
//
// The purpose of this get the first positive solution on solve the 
//  Diophantine Equation 
//     Bxy + Dx + Ey = F
// Here I put some variants for test how fast they are

using System;
using System.Collections.Generic;

namespace lz.math.solver
{
    // Simple Hyperbolic case A = C = 0; B ≠ 0
    // Ax2 + Bxy + Cy2 + Dx + Ey=F
    // Methods to solve Bxy + Dx + Ey=F
    public class HyperSolver
    {
        public ulong B;
        public ulong D;
        public ulong E;

        /* This solve check if the solution is on the center, and from there start to walk up and down for found the solution */
        public Result EquSolverV1(ulong f)
        {
            Result res = new Result { x = 0, y = 0 };
            ulong r;
            double st = DiagonalSolver(f);
            ulong xu, yu, xd, yd;
            xu = yu = xd = yd = (ulong)st;
            int nsol = 0;
            while (nsol != 3)
            {
                if (xu != ulong.MaxValue)
                {
                    r = Eval(xu, yu);
                    if (r == f)
                    {
                        res.x = xu;
                        res.y = yu;
                        break;
                    }
                    else if (r < f)
                    {
                        yu++;
                    }
                    else
                    {
                        xu--;
                    }
                }
                else
                {
                    nsol |= 1;
                }

                if (yd != ulong.MaxValue)
                {
                    r = Eval(xd, yd);
                    if (r == f)
                    {
                        res.x = xd;
                        res.y = yd;
                        break;
                    }
                    else if (r < f)
                    {
                        xd++;
                    }
                    else
                    {
                        yd--;
                    }
                }
                else
                {
                    nsol |= 2;
                }
            }
            return res;
        }

        /** This solve check if the solution is on the center, and from there start to walk up and down for found the solution, but jump some calculation using the distance from the result
                */
        public Result EquSolverV2(ulong f)
        {
            Result res = new Result { x = 0, y = 0 };
            double st = DiagonalSolver(f);
            ulong xu, yu, xd, yd;
            xu = yu = xd = yd = (ulong)st;
            int nsol = 0;
            int test = 0;
            double dist;
            ulong r;
            while (nsol != 3)
            {
                if (xu != ulong.MaxValue)
                {
                    r = Eval(xu, yu);
                    test++;
                    if (r == f)
                    {
                        res.x = xu;
                        res.y = yu;
                        break;
                    }
                    else if (r < f)
                    {
                        dist = (double)(f - r) / (double)YNext(yu);
                        dist = Math.Floor(dist) + 1;
                        yu += (ulong)dist;
                    }
                    else
                    {
                        dist = (double)(r - f) / (double)XNext(xu);
                        dist = Math.Floor(dist) + 1;
                        xu -= (ulong)dist;
                    }
                }
                else
                {
                    nsol |= 1;
                }

                if (yd != ulong.MaxValue)
                {

                    r = Eval(xd, yd);
                    test++;
                    if (r == f)
                    {
                        res.x = xd;
                        res.y = yd;
                        break;
                    }
                    else if (r < f)
                    {
                        dist = (double)(f - r) / (double)XNext(xd);
                        dist = Math.Floor(dist) + 1;
                        xd += (ulong)dist;
                    }
                    else
                    {
                        dist = (double)(r - f) / (double)YNext(yd);
                        dist = Math.Floor(dist) + 1;
                        yd -= (ulong)dist;
                    }
                }
                else
                {
                    nsol |= 2;
                }
            }
            return res;
        }

        /* This solver goes for all the Y intil found a X that mach the value */
        public Result EquSolverV3(ulong f)
        {
            Result res = new Result { x = 0, y = 0 };
            ulong maxY = (f - 1) / 7;
            ulong y = 0;
            while (y < maxY)
            {
                ulong x = SolveX(f, y);
                if (x > 0)
                {
                    res.x = x;
                    res.y = y;
                    break;
                }
                y++;
            }
            return res;
        }

        /** This solver goes for all the X intil found a Y that mach the value */
        public Result EquSolverV4(ulong f)
        {
            Result res = new Result { x = 0, y = 0 };
            ulong maxX = (f - 1) / 5;
            ulong x = 0;
            while (x < maxX)
            {
                ulong y = SolveY(f, x);
                if (y > 0)
                {
                    res.x = x;
                    res.y = y;
                    break;
                }
                x++;
            }
            return res;
        }

        /* I make a litle change for not negative solution, the idea comming from
         https://www.alpertron.com.ar/METHODS.HTM#SHyperb
         */
        public Result EquSolverV5(ulong f)
        {
            Result res = new Result { x = 0, y = 0 };
            ulong k = D * E + B * f;  // BXY+DX+EY=f

            if (k != 0)
            {
                ulong S = (ulong)Math.Sqrt((double)k);
                foreach (ulong t in PosiblePrimes())
                {
                    if (t > S)
                    {
                        break;
                    }
                    if (k % t == 0)
                    {
                        if (SolByFact(k, t, ref res))
                        {
                            break;
                        }
                        if ((t * t != k) && (SolByFact(k, k / t, ref res)))
                        {
                            break;
                        }
                    }
                }
            }
            return res;
        }

        /* A simple c% way to get posibles prime number using the Wheel 
         https://www.codeproject.com/Articles/31085/Prime-Number-Determination-Using-Wheel-Factorizati 
         */
        private static ulong[] aV30 = { 7, 11, 13, 17, 19, 23, 29, 31 };
        private static ulong[] fVal = { 1, 2, 3, 5 }; // I add 1 because is need for this solver
        public IEnumerable<ulong> PosiblePrimes()
        {
            foreach (ulong p in fVal)
            {
                yield return p;
            }
            ulong pass= 0;
            while (pass<(ulong.MaxValue-30))
            {
                foreach (ulong sieve in aV30)
                {
                    yield return pass + sieve;
                }
                pass += 30;
            }
        }

        // for EquSolverV5
        private bool SolByFact(ulong r, ulong t, ref Result res)
        {
            if ((t - E) % B == 0 && (r / t - D) % B == 0)
            {
                return AddXY((t - E) / B, (r / t - D) / B, ref res);
            }
            return false;
        }

        // for EquSolverV5
        private bool AddXY(ulong x, ulong y, ref Result res)
        {
            if ((x >= 0) && (y >= 0))
            {
                res.x = x;
                res.y = y;
                return true;
            }
            return false;
        }

        /* 
         * @Eval
            Giving a X and Y return a F value
        */
        public ulong Eval(ulong x, ulong y) => B * x * y + D * x + E * y;

        /* Depending of the position of X the distance to the next X
         */
        public ulong XNext(ulong x) => B * x + E;

        /* Depending of the position of Y the distance to the next Y 
         */
        public ulong YNext(ulong y) => B * y + D;

        /* Giving an Y value, check if the result is on that column 
         */
        ulong SolveX(ulong e, ulong y)
        {
            ulong num = e - E * y;
            ulong den = B * y + D;
            if ((num % den) == 0)
            {
                return num / den;
            }
            return 0;
        }

        /* Giving an X value, check if the result is on that row
         */
        ulong SolveY(ulong e, ulong x)
        {
            ulong num = e - D * x;
            ulong den = B * x + E;
            if ((num % den) == 0)
            {
                return num / den;
            }
            return 0;
        }

        /* Assuming that x = and what possible value would have
         */
        public double DiagonalSolver(ulong f) => (Math.Sqrt(B) * Math.Sqrt(f + B) - B) / B;
    }

    public struct Result
    {
        public ulong x;
        public ulong y;
    }

}
