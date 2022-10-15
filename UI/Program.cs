

namespace interfaceUi
{    
    public class Program
    {
        public static void Main(string[] args)
        {
            if(args.Length > 0)
                Console.WriteLine(args[0]);

            var teste = new Graph.Link<Test>(new Test(), new Test());

            Console.WriteLine("==============[");
            Console.WriteLine(teste);
            Console.WriteLine("]==============");
        }

    }
}


public class Test 
{
    public int Id { get; set; }
}



