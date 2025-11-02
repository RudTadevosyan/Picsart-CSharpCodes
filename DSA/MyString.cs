namespace MyString;

public class Program
{
    public enum Option
    {
        IgnoreLeadingSpace = 1,
        CaseInSensitive = 2
    };

    public partial class MyString
    {
        private int _size = 5;
        private char[] _string;
        
        public bool Empty => _string.Length == 0;
        public int Length => _string.Length;

        public char this[int index]
        {
            get => _string[index];
        }
        
        public MyString()
        {
            _string = new char[_size];  
        }
        public MyString(char[] str)
        {
            _string = new char[str.Length];
            for (int i = 0; i < _string.Length; i++)
            {
                _string[i] = str[i];
            }
        }

        public MyString(char ch, int count)
        {
            _string = new char[count];
            for (int i = 0; i < count; i++)
            {
                _string[i] = ch;
            }
        }

        public MyString(char[] str, int index, int length)
        {
            _string = new char[length - index];
            for (int i = 0; i < _string.Length; i++)
            {
                _string[i] = str[index + i];
            }
        }
        
        public override string ToString()
        {
            if (_string.Length == 0) return string.Empty;
            return new string(_string);
        }
    }

    public partial class MyString
    {
        public static int Compare(MyString str1, MyString str2)
        {
            if (str1.Length != str2.Length) return -1;

            for (int i = 0; i < str1.Length; i++)
            {
                if (str1._string[i] != str2._string[i]) return (str1._string[i] - str2._string[i]);
            }

            return 0;
        }

        public static int Compare(MyString str1, MyString str2, Option op)
        {
            if (op == Option.CaseInSensitive) return CaseInsensitiveComparer(str1, str2);
            if (op == Option.IgnoreLeadingSpace) return IgnoreLeadingSpaceComparer(str1, str2);
            return -1;
        }

        private static int CaseInsensitiveComparer(MyString str1, MyString str2)
        {
            if (str1.Length != str2.Length) return -1;

            for (int i = 0; i < str1.Length; i++)
            {
                char ch1 = char.ToLower(str1._string[i]);
                char ch2 = char.ToLower(str2._string[i]);

                if (ch1 != ch2) return (ch1 - ch2);
            }

            return 0;
        }

        private static int IgnoreLeadingSpaceComparer(MyString str1, MyString str2)
        {
            int index1 = 0;
            int index2 = 0;

            while (index1 < str1.Length && str1._string[index1] == ' ') index1++;
            while (index2 < str2.Length && str2._string[index2] == ' ') index2++;

            MyString strA = new MyString(str1._string, index1, str1.Length);
            MyString strB = new MyString(str2._string, index2, str2.Length);

            return Compare(strA, strB);
        }

        public static bool operator ==(MyString str1, MyString str2)
        {
            return Compare(str1, str2) == 0;
        }

        public static bool operator !=(MyString str1, MyString str2)
        {
            return Compare(str1, str2) != 0;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is MyString)) return false;
            return Compare(this, (MyString)obj) == 0;
        }

        public static bool Equals(MyString str1, MyString str2)
        {
            return str1 == str2;
        }
        
        public override int GetHashCode()
        {
            int hash = 17;
            foreach (char c in _string)
            {
                hash = hash * 31 + c + 13;
            }
            return hash;
        }

        public static MyString Join(MyString separator, params MyString[] myStrings)
        {
            if (myStrings.Length == 0)
            {
                return new MyString(new char[0]);
            }
            
            int length = separator.Length * (myStrings.Length - 1);

            foreach (MyString myString in myStrings)
            {
                length += myString.Length;
            }
            
            char[] result = new char[length];
            int k = 0;
            
            for (int i = 0; i < myStrings.Length; i++)
            {
                for (int j = 0; j < myStrings[i].Length; j++)
                {
                    result[k++] = myStrings[i]._string[j];
                }

                if (i < myStrings.Length - 1)
                {
                    for (int j = 0; j < separator.Length; j++)
                    {
                        result[k++] = separator._string[j];
                    }
                }
            }

            return new MyString(result);
        }

        public bool StartsWith(MyString str1)
        {
            if(this.Length < str1.Length) return false;

            for (int i = 0; i < str1.Length; i++)
            {
                if (this._string[i] != str1._string[i]) return false;
            }
            
            return true;
        }

        public bool EndsWith(MyString str1)
        {
            if(this.Length < str1.Length) return false;

            for (int i = 1; i <= str1.Length; i++)
            {
                if (this._string[^i] != str1._string[^i]) return false;
            }
            
            return true;
        }

        public static MyString Concat(MyString str1, MyString str2)
        {
            if(str1.Length == 0 && str2.Length == 0) return new MyString(new char[0]);
            if(str1.Length == 0 && str2.Length != 0) return new MyString(str2._string);
            if(str1.Length != 0 && str2.Length == 0) return new MyString(str1._string);
            
            int totalLength = str1.Length + str2.Length;
            char[] result = new char[totalLength];
            int k = 0;

            for (int i = 0; i < str1.Length; i++)
            {
                result[k++] = str1._string[i];
            }

            for (int i = 0; i < str2.Length; i++)
            {
                result[k++] = str2._string[i];
            }
            
            return new MyString(result);
            
        }
        
        public static MyString operator +(MyString str1, MyString str2)
        {
            return Concat(str1, str2);
        }
        
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("1:");
        MyString str1 = new MyString("Hello".ToCharArray());
        MyString str2 = new MyString("World".ToCharArray());
        MyString str3 = new MyString("   Leading Space".ToCharArray());
        MyString str4 = new MyString("   Leading Space".ToCharArray());

        Console.WriteLine(str1);
        Console.WriteLine(str2);
        Console.WriteLine(str3);
        
        Console.WriteLine("\n2:");

        Console.WriteLine(str1 == str2);
        Console.WriteLine(str1 != str2);
        
        Console.WriteLine("\n3:");

        Console.WriteLine(str3.Equals(str4));
        
        Console.WriteLine("\n4:");

        MyString str5 = new MyString("   Hello".ToCharArray());
        MyString str6 = new MyString("Hello".ToCharArray());

        Console.WriteLine(MyString.Compare(str5, str6, Option.IgnoreLeadingSpace));

        MyString str7 = new MyString("HELLO".ToCharArray());
        MyString str8 = new MyString("hello".ToCharArray());

        Console.WriteLine(MyString.Compare(str7, str8, Option.CaseInSensitive));
        
        Console.WriteLine("\n5:");

        MyString separator = new MyString(", ".ToCharArray());
        MyString result = MyString.Join(separator, str1, str2, str3);
        Console.WriteLine(result);
        
        Console.WriteLine("\n6:");

        MyString str9 = new MyString("Good".ToCharArray());
        MyString str10 = new MyString("Morning".ToCharArray());
        MyString concatResult = str9 + str10;
        Console.WriteLine(concatResult);
        
        Console.WriteLine("\n7:");

        MyString str11 = new MyString("Hello".ToCharArray());
        MyString prefix = new MyString("Hel".ToCharArray());
        Console.WriteLine(str11.StartsWith(prefix));
        
        Console.WriteLine("\n8:");

        MyString str12 = new MyString("Goodbye".ToCharArray());
        MyString suffix = new MyString("bye".ToCharArray());
        Console.WriteLine(str12.EndsWith(suffix));

    }
}
