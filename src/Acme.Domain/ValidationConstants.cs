namespace Acme.Domain
{
    public static class ValidationConstants
    {
        public static class Submission
        {
            public static class FirstName
            {
                public const int MinLength = 1;
                public const int MaxLength = 50;
            }

            public static class LastName
            {
                public const int MinLength = 1;
                public const int MaxLength = 50;
            }
            
            public static class Birthday
            {
                public const int MinimumAgeNumberOfInYears = 18;
            }

                 public static class SerialNumberCode
            {
                public const int MaxNumberOfSerialNumberUsages = 2;
            }

           
        }
    }
}