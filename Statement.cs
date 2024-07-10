 public class Statement
    {
        public string konkurs, nrecab, abitplanfin;
        public int prioritet;
        public UInt64 a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19;
        //public List<UInt64> param = new List<ulong>();
        public Statement(string nrecab,string konkurs, string abitplanfin, int prioritet, List<UInt64> a)
        {
            this.nrecab = nrecab;
            this.konkurs = konkurs;
            this.prioritet = prioritet;
            this.abitplanfin = abitplanfin; 

            a0 = a1 = a2 = a3 = a4 = a5 = a6 = a7 = a8 = a9 = a10 = a11 = a12 = a13 = a14 = a15 = a16 = a17 = a18 = a19 = 0;
            //param = a;
            for (int i = 0; i < a.Count; i++)
            {
                if (i == 0) a0 = a[i];
                if (i == 1) a1 = a[i];
                if (i == 2) a2 = a[i];
                if (i == 3) a3 = a[i];
                if (i == 4) a4 = a[i];
                if (i == 5) a5 = a[i];
                if (i == 6) a6 = a[i];
                if (i == 7) a7 = a[i];
                if (i == 8) a8 = a[i];
                if (i == 9) a9 = a[i];
                if (i == 10) a10 = a[i];
                if (i == 11) a11 = a[i];
                if (i == 12) a12 = a[i];
                if (i == 13) a13 = a[i];
                if (i == 14) a14 = a[i];
                if (i == 15) a15 = a[i];
                if (i == 16) a16 = a[i];
                if (i == 17) a17 = a[i];
                if (i == 18) a18 = a[i];
                if (i == 19) a19 = a[i];
            }


        }
    }
