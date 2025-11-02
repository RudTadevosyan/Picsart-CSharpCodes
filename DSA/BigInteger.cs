namespace BigInt;

class Program
{
    public class BigInt
    {
        private string _number = null!;
        private bool _signFlag = false;
        public int Length
        {
            get
            {
                return _number.Length;
            }
        }
        public BigInt(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                _number = "0";
                return;
            }


            if (number[0] == '-')
            {
                _signFlag = true;
                number = number.Substring(1);
            }
            
            foreach (char n in number)
            {
                if (!char.IsDigit(n))
                { 
                    Console.WriteLine("Invalid number"); 
                    Environment.Exit(0);
                } 
            }
            
            number = number.TrimStart('0');
            if (number == "") number = "0";
            
            _number = number;
        }

        public override string ToString() 
        {
            if (_signFlag)
            {
                return '-' + _number;
            }
            
            return _number;
        }
        
        public static BigInt operator +(BigInt a, BigInt b)
                {
                    if (a._signFlag == false && b._signFlag == false)
                    {
                        BigInt result = new BigInt(Addition(a._number, b._number));
                        result._signFlag = false;

                        return result;
                    }
                    else if (a._signFlag == true && b._signFlag == true)
                    {
                        BigInt result = new BigInt(Addition(a._number, b._number));
                        result._signFlag = true;

                        return result;
                    }
                    else
                    {
                        if (CompareAbsForFirst(a._number, b._number))
                        {
                            BigInt result = new BigInt(SubtractionFromFirst(a._number, b._number));
                            result._signFlag = a._signFlag;

                            return result;
                        }
                        else
                        {
                            BigInt result = new BigInt(SubtractionFromFirst(b._number, a._number));
                            result._signFlag = result._number != "0" && b._signFlag;
                            
                            return result;
                        }
                        
                    }
                }
        
        public static BigInt operator -(BigInt a, BigInt b)
        {
            if (a._signFlag == false && b._signFlag == false)
            {
                if (CompareAbsForFirst(a._number, b._number))
                {
                    BigInt result = new BigInt(SubtractionFromFirst(a._number, b._number));
                    result._signFlag = false;

                    return result;
                }
                else
                {
                    BigInt result = new BigInt(SubtractionFromFirst(a._number, b._number));
                    result._signFlag = true;

                    return result;
                }
            }

            else if (a._signFlag == false && b._signFlag == true)
            {
                BigInt result = new BigInt(Addition(a._number, b._number));
                result._signFlag = false;
                
                return result;
            }
            else if (a._signFlag == true && b._signFlag == false)
            {
                BigInt result = new BigInt(Addition(a._number, b._number));
                result._signFlag = true;
                
                return result;
            }
            else
            {
                BigInt result = new BigInt(SubtractionFromFirst(b._number, a._number));
                result._signFlag = (!CompareAbsForFirst(b._number, a._number));
                
                return result;
            }
        }

        public static BigInt operator *(BigInt a, BigInt b)
        {
            if ((a._signFlag == false && b._signFlag == false) || (a._signFlag == true && b._signFlag == true))
            {
                BigInt result = new BigInt(Multiplication(a._number, b._number));
                result._signFlag = false;
                
                return result;
            }
            else
            {
                BigInt result = new BigInt(Multiplication(a._number, b._number));
                result._signFlag = true;
                
                return result;
            }
        }
        

        public static bool operator >(BigInt a, BigInt b)
        {
            return a.CompareTo(b);
        }
        
        public static bool operator <(BigInt a, BigInt b)
        {
            return b.CompareTo(a);
        }

        public static bool operator ==(BigInt a, BigInt b)
        {
            if (a.CompareTo(b) || b.CompareTo(a))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        public static bool operator !=(BigInt a, BigInt b)
        {
            if (a.CompareTo(b) || b.CompareTo(a))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string SubtractionFromFirst(string a, string b)
        {
            int aLength = a.Length;
            int bLength = b.Length;

            int finalNumberLength = (aLength > bLength) ? aLength : bLength;
            char[] finalNumber = new char[finalNumberLength];
            int borrow = 0;

            while (aLength > 0 || bLength > 0)
            {
                int x = (aLength > 0) ? a[--aLength] - '0' : 0;
                int y = (bLength > 0) ? b[--bLength] - '0' : 0;

                x -= borrow;

                if (x < y)
                {
                    borrow = 1;
                    x += 10;
                }
                else
                {
                    borrow = 0;
                }
                
                finalNumber[--finalNumberLength] = (char)(x - y + '0');

            }
            
            return new string(finalNumber);
        }
        private static string Addition(string a, string b)
        {
            int aLength = a.Length;
            int bLength = b.Length;

            int maxLength = (aLength > bLength) ? aLength : bLength;

            int finalNumberLength = maxLength + 1; 
            char[] finalNumber = new char[finalNumberLength];

            int carry = 0;

            while (aLength > 0 || bLength > 0 || carry > 0)
            {
                if (aLength > 0)
                {
                    carry += a[--aLength] - '0';
                }

                if (bLength > 0)
                {
                    carry += b[--bLength] - '0';
                }

                finalNumber[--finalNumberLength] = (char)((carry % 10) + '0');
                carry /= 10;
            }

            return new string(finalNumber, finalNumberLength, finalNumber.Length - finalNumberLength);
        }
        private static string Multiplication(string a, string b)
        {
            int aLength = a.Length;
            
            int finalNumberLength = a.Length + b.Length;
            char[] finalNumber = new char[finalNumberLength];

            for (int i = 0; i < b.Length; i++)
            {
                int temp = finalNumberLength;
                int carry = 0;
                int digit = b[b.Length - 1 - i] - '0';
                
                while (aLength > 0 || carry > 0) // mult to 1 digit
                {
                    if (aLength > 0) carry += digit * (a[--aLength] - '0');

                    if (finalNumber[temp - 1 - i] != '\0') carry += finalNumber[temp - 1 - i] - '0';

                    finalNumber[--temp - i] = (char)(carry % 10 + '0');
                    carry /= 10;
                }

                temp = finalNumberLength;
                aLength = a.Length;
                carry = 0;
            }

            
            string result =new string(finalNumber).TrimStart('0', '\0');
            if(result == "") result = "0";
            
            return result;
        }
        
        private static bool CompareAbsForFirst(string a, string b)
        {
            if (a.Length > b.Length)
            {
                return true;
            }
            else if (a.Length == b.Length)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    if (a[i] > b[i])
                    {
                        return true;
                    }
                    else if (a[i] < b[i])
                    {
                        return false;
                    }
                }
                
                return false;
            }
            else
            {
                return false;
            }
        }    
        public bool CompareTo(BigInt a)
        {
            if(_signFlag == false && a._signFlag == true) return true;
            else if (_signFlag == true && a._signFlag == false) return false;
            else
            {
                if (_signFlag == true)
                {
                    return !CompareAbsForFirst(_number, a._number);
                }
                else
                {
                    return CompareAbsForFirst(_number, a._number);
                }
            }
        }

        public override bool Equals(object? a)
        {
            if(a is null) return false;
            return this == (BigInt)a;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode() ^ this.Length.GetHashCode() ^ 17;
        }

        public string Abs()
                 {
                     return _number;
                 }
    }

    static void Main()
    {

        string? number1;
        do
        {
            Console.Write("Input a number: ");
            number1 = Console.ReadLine();
        }while(string.IsNullOrEmpty(number1));
        
        string? number2;
        do
        {
            Console.Write("Input a number: ");
            number2 = Console.ReadLine();
        }while(string.IsNullOrEmpty(number2));
        
        BigInt a = new BigInt(number1);
        BigInt b = new BigInt(number2);
        
        Console.WriteLine($"\nMultiplication: {a * b}");
        Console.WriteLine($"Addition: {a + b}");
        Console.WriteLine($"Subtraction: {a - b}");
        
        Console.WriteLine($"\nfirst number length: {a.Length}");
        Console.WriteLine($"second number length: {b.Length}");
        
        Console.WriteLine($"a > b: {a > b}");
        Console.WriteLine($"a < b: {a < b}");
        Console.WriteLine($"a == b: {a == b}");
        Console.WriteLine($"a != b: {a != b}");
        
    } 
}
