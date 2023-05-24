using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaluaLib
{
    public class Galuafield
    {
        private int q;
        private int m;
        public Dictionary<int, List<int>> dict = new Dictionary<int, List<int>>();
        public Galuafield(int qq, int mm)
        {
            try
            {
                q = qq;
                if (!IsPrime(q))
                    throw new ArgumentException("Please, enter prime q.");
                m = mm;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
       

        public void building_field(int qq, int mm, List<int> g)
        {
            int qm = (int) Math.Pow(qq,mm)-1;

            int[] mass = { 1 };// = 1
            int[] x = { 0, 1};
            List<int> value_poly = mass.ToList<int>();
            List<int> multiplier = x.ToList<int>();
            dict.Add(0, value_poly);
            for (int i = 1; i < qm; i++)
            {
                value_poly = multiply_usual(value_poly, multiplier);
                value_poly = division_usual_2_0(value_poly, g);
                value_poly = fit(value_poly);
                if ((value_poly[0] == 1) && (value_poly.Count() == 1))
                {
                    dict.Clear();
                    return;
                }
                dict.Add(i, value_poly);
                //dict.Add(dict.Count,richTe)
            }
        }
        private List<int> fit(List<int>a)
        {
            while (a.Last() == 0)
                a.RemoveAt(a.Count - 1);
            return a;
        }
        public List<int> add(List<int> a, List<int> b)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < m; i++)
            {
                result.Add((a[i] + b[i]) % q);
            }
            return result;
        }

        public List<int> substract(List<int> a, List<int> b)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < m; i++)
            {
                result.Add(Math.Abs(a[i] - b[i]) % q);
            }
            return result;
        }
        public int calcSelf(List<int> a, int x)
        {
            int result = 0;
            for(int i=0;i<a.Count;i++)
            {
                result += a[i] * (int)Math.Pow(x, i);
            }
            return result % q;
        }
        private List<int> multiply_usual(List<int> a, List<int> b)
        {
            int[] arr = new int[a.Count + b.Count - 1];
            List<int> result = arr.ToList();
            for (int i = 0; i < a.Count(); i++)
            {
                for (int j = 0; j < b.Count(); j++)
                {
                    result[i + j] += (a[i] * b[j]) % q;
                }
            }
            return result;
        }
        public List<int> division_usual_2_0(List<int> g, List<int> x)//g - delimoe
        {
            List<int> res = new List<int>();
            if (g.Count < x.Count)
            {
                return g;
            }
            while (x.Last() != 1)
            {
                x.RemoveAt(x.Count - 1);
            }
            while (g.Count > x.Count - 1)
            {
                List<int> del = new List<int>();
                if (g.Count != x.Count - 1)
                {//домножать
                    for (int i = 0; i < g.Count - x.Count; i++)
                        del.Add(0);
                    for (int i = 0; i < x.Count; i++)
                        del.Add(x[i]);
                }
                int mn = g.Count / del.Count;
                res.Add(mn);
                for (int i = 0; i < g.Count; i++)
                {
                    g[i] = g[i] - del[i] * mn;
                    if (g[i] == -1)
                        g[i] = 1;
                }
                g.RemoveAt(g.Count - 1);
            }
            return g;
        }
        private List<int> division_usual(List<int> a, List<int> b)
        {
            int deg = (a.Count - 1);
            int bdeg = (b.Count - 1);
            int dif = deg - bdeg;
            if (dif < 0)
            {
                return a;
            }
            int[] mass = new int[dif+1];
            List<int> temp = a;
            List<int> res = new List<int>();
            for (int i = deg; i >= 0; i--)
            {
                res.Add(temp[i] / b[bdeg]);

                for (int j = i - 1; j >= 0; j--)
                {
                    temp[j] -= res[i] * b[j - i];
                }
            }
            //temp.setdeg(deg - dif - 1);
            return temp;
        }
        public List<int> multiply(List<int> a, List<int> b)
        {
            int qm = (int)Math.Pow(q, m) - 1;
            int k = dict.Where(x => x.Value.SequenceEqual(a)).FirstOrDefault().Key;
            int n = dict.Where(x => x.Value.SequenceEqual(b)).FirstOrDefault().Key;
            int res = (k + n) % (qm - 1);
            List<int> value;
            dict.TryGetValue(res, out value);
            return value;
        }
        public List<int> division(List<int> a, List<int> b)
        {
            int qm = (int)Math.Pow(q, m) - 1;
            int k = dict.Where(x => x.Value.SequenceEqual(fit(a))).FirstOrDefault().Key;
            int n = dict.Where(x => x.Value.SequenceEqual(fit(b))).FirstOrDefault().Key;
            int kn = k - n;
            int res = kn % qm;
            if (res < 0)
            {
                res += qm;
            }
            List<int> value;
            dict.TryGetValue(res, out value);
            return value;
        }
        public static bool IsPrime(int n)
        {
            if (n < 2)
            {
                return false;
            }
            if (n == 2 || n == 3)
            {
                return true;
            }
            if (n % 2 == 0)
            {
                return false;
            }

            // Мал т Ферма a^(p-1) ≡ 1 (mod p) 
            // Выбираем случайное a и проверяем условие для него
            Random rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                int a = rand.Next(2, n - 1);
                if (ModPow(a, n - 1, n) != 1)
                {
                    return false;
                }
            }
            return true;
        }

        // Возведение в степень по модулю.
        public static int ModPow(int a, int b, int n) /*возведение числа a в степень b по модулю n*/
        {
            int res = 1;
            while (b > 0)
            {
                if ((b & 1) != 0)
                {
                    res = (res * a) % n;
                }
                a = (a * a) % n;
                b >>= 1;
            }
            return res;
        }
    }
}
