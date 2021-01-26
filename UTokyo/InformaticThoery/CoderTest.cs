namespace UTokyo.InformaticThoery
{
    public class CoderTest
    {
        public static void HuffmanEnCoderTest()
        {
            var nums = new[] {4, 4, 2, 45, 5, 1, 44, 28};
            var dict = HuffmanEnCoder.Encode(nums);
            var data = HuffmanEnCoder.Decode("011101011011100000110011", dict);
            data.PrintCollectionToConsole();
        }
    }
}