using System.Text;

namespace ServiceElectronicQueue
{
    public static class Rand
    {
        private static string[] ARR = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "U", "Z" };
        private static int[] INT = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public static string Str(int size)
        {
            Random rand = new ();
            StringBuilder str = new ("");
            for (int i = 0; i <= size; i++)
            {
                int r = rand.Next(0, 1);

                if (r == 0)
                {
                    str.Insert(i, ARR[rand.Next(0, 25)]);
                }
                else if (r == 1)
                {
                    str.Insert(i, INT[rand.Next(0, 9)].ToString());
                }
            }
            return str.ToString();
        }
    }
}