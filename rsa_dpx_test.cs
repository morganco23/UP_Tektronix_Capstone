using System;

namespace rsa_dpx_test {
    static class Runner {

        public static void search_connect(){
            Console.WriteLine("Searching for device...");
            
            // import Connor's code
        }

        public static void graph_axis(float cf, int refLevel, float span, float rbw){
            Console.WriteLine("Graphing...");

            Tuple<float, float> dpx_values = new Tuple<float, float>();
            
        }

        public static void dpx_example(){
            search_connect();

            string[] args =  Environment.GetCommandLineArgs();
            if(args.Length != 5){
                Console.WriteLine("Minimum of 5 command line arguments required.");
            }

            float cf = float.Parse(args[1]); // center frequency
            int refLevel = int.Parse(args[2]); // reference level
            float span = float.Parse(args[3]); // span
            float rbw = float.Parse(args[4]); // resolution bandwidth
            
            graph_axis(cf, refLevel, span, rbw);

        
        }






        






        static void Main(String[] args){

            Runner.dpx_example();
        }
    }
}