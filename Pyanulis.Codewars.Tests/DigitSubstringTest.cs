using NUnit.Framework;
using System.Diagnostics;
using Pyanulis.Codewars.Algo;

namespace Pyanulis.Codewars.Tests
{
    public class DigitSubstringTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test123456789()
        {
            string substring = "123456789";
            var chunks = DigitSubstring.GetChunks(substring, 1, 0);
            int i = 1;
            foreach(var chunk in chunks)
            {
                Debug.WriteLine(chunk);
                Assert.AreEqual(i++, chunk.ToNum());
            }

            Assert.AreEqual(0L.Digits(), 1);
            Assert.AreEqual(1L.Digits(), 1);
            Assert.AreEqual(10L.Digits(), 2);
            Assert.AreEqual(111L.Digits(), 3);
            Assert.AreEqual(123456789L.Digits(), 9);
        }

        [Test]
        public void TestDigits()
        {
            Assert.AreEqual(0L.Digits(), 1);
            Assert.AreEqual(1L.Digits(), 1);
            Assert.AreEqual(10L.Digits(), 2);
            Assert.AreEqual(111L.Digits(), 3);
            Assert.AreEqual(123456789L.Digits(), 9);
        }

        [Test]
        public void TestOrder()
        {
            Assert.IsTrue(1L.CanBePrevTo(2L));
            Assert.IsTrue(11L.CanBePrevTo(12));
            Assert.IsTrue(1L.CanBePrevTo(12));
            Assert.IsTrue(12L.CanBePrevTo(1));
            Assert.IsTrue(3L.CanBePrevTo(204));
            Assert.IsTrue(204L.CanBePrevTo(20));

            Assert.IsFalse(1L.CanBePrevTo(3L));
            Assert.IsFalse(11L.CanBePrevTo(13));
            Assert.IsFalse(1L.CanBePrevTo(23));
            Assert.IsFalse(23L.CanBePrevTo(1));
            Assert.IsFalse(2L.CanBePrevTo(204));
            Assert.IsFalse(204L.CanBePrevTo(10));
        }

        [Test]
        public void TestChunkOrder()
        {
            string s = "123456789";
            Assert.IsTrue(DigitSubstring.IsInOrder(DigitSubstring.GetChunks(s, 1, 0), 1), s);
            Assert.IsFalse(DigitSubstring.IsInOrder(DigitSubstring.GetChunks(s, 2, 0), 2));

            s = "121314151617";
            Assert.IsTrue(DigitSubstring.IsInOrder(DigitSubstring.GetChunks(s, 2, 0), 2), s);
            Assert.IsFalse(DigitSubstring.IsInOrder(DigitSubstring.GetChunks(s, 3, 0), 3), s);

            s = "21314151617"; // 12,13,14,15,16,17
            Assert.IsTrue(DigitSubstring.IsInOrder(DigitSubstring.GetChunks(s, 2, 1), 2), s);
            Assert.IsFalse(DigitSubstring.IsInOrder(DigitSubstring.GetChunks(s, 2, 0), 2), s);

            s = "21322323324325"; // 321, 322, 323, 324, 325
            Assert.IsTrue(DigitSubstring.IsInOrder(DigitSubstring.GetChunks(s, 3, 2), 3), s);
            Assert.IsFalse(DigitSubstring.IsInOrder(DigitSubstring.GetChunks(s, 3, 1), 3), s);

            s = "12312412512";
            Assert.IsTrue(DigitSubstring.IsInOrder(DigitSubstring.GetChunks(s, 3, 0), 3), s);
            Assert.IsFalse(DigitSubstring.IsInOrder(DigitSubstring.GetChunks(s, 3, 1), 3), s);

            s = "2212312412512";
            Assert.IsTrue(DigitSubstring.IsInOrder(DigitSubstring.GetChunks(s, 3, 2), 3), s);
            Assert.IsFalse(DigitSubstring.IsInOrder(DigitSubstring.GetChunks(s, 3, 1), 3), s);

            s = "0404";
            Assert.IsTrue(DigitSubstring.IsInOrder(DigitSubstring.GetChunks(s, 4, 3), 4), s);
            Assert.IsFalse(DigitSubstring.IsInOrder(DigitSubstring.GetChunks(s, 2, 0), 2), s);

            s = "910";
            //Assert.IsTrue(DigitSubstring.IsInOrder(DigitSubstring.GetChunks(s, 1, 0), 1), s);

            s = "1234567891";
            Assert.IsTrue(DigitSubstring.IsInOrder(DigitSubstring.GetChunks(s, 1, 0), 1), s);

            s = "8910";
            Assert.IsTrue(DigitSubstring.IsInOrder(DigitSubstring.GetChunks(s, 1, 0), 1), s);
        }

        [Test]
        public void TestGetMin()
        {
            string s = "123456789";
            Assert.AreEqual(1, DigitSubstring.GetMin(DigitSubstring.GetChunks(s, 1, 0), 1));

            s = "121314151617";
            Assert.AreEqual(12, DigitSubstring.GetMin(DigitSubstring.GetChunks(s, 2, 0), 2));

            s = "21314151617"; // 12,13,14,15,16,17
            Assert.AreEqual(12, DigitSubstring.GetMin(DigitSubstring.GetChunks(s, 2, 1), 2));

            s = "21322323324325"; // 321, 322, 323, 324, 325
            Assert.AreEqual(321, DigitSubstring.GetMin(DigitSubstring.GetChunks(s, 3, 2), 3));

            s = "12312412512"; // 123, 124, 125, 126
            Assert.AreEqual(123, DigitSubstring.GetMin(DigitSubstring.GetChunks(s, 3, 0), 3));

            s = "2212312412512"; // 122, 123, 124, 125, 126
            Assert.AreEqual(122, DigitSubstring.GetMin(DigitSubstring.GetChunks(s, 3, 2), 3));
        }

        [Test]
        public void TestMerge()
        {
            Assert.AreEqual(3536, DigitSubstring.Merge(536, 35, 4));
            Assert.AreEqual(400, DigitSubstring.Merge(0, 40, 3));
            Assert.AreEqual(4040, DigitSubstring.Merge(0, 404, 4));
            Assert.AreEqual(4040, DigitSubstring.Merge(040L, 4, 4));
        }

        [Test]
        public void TestAlgo()
        {
            Assert.AreEqual(24674951477L, DigitSubstring.FindByChunks("58257860625")); //2578606258->2578606259
            Assert.AreEqual(13034, DigitSubstring.FindByChunks("53635"));
            Assert.AreEqual(1091, DigitSubstring.FindByChunks("040"));
            Assert.AreEqual(168, DigitSubstring.FindByChunks("99"));
            Assert.AreEqual(15050, DigitSubstring.FindByChunks("0404"));
            Assert.AreEqual(6957586376885L, DigitSubstring.FindByChunks("3999589058124")); //589058123999->589058124000
            Assert.AreEqual(1686722738828503L, DigitSubstring.FindByChunks("555899959741198")); //119855589995974->119855589995975
            Assert.AreEqual(2927, DigitSubstring.FindByChunks("0910"));
            Assert.AreEqual(2617, DigitSubstring.FindByChunks("0991"));
            Assert.AreEqual(2617, DigitSubstring.FindByChunks("09910"));
            Assert.AreEqual(35286, DigitSubstring.FindByChunks("09991"));


            Assert.AreEqual(3, DigitSubstring.FindByChunks("456"), "...456...");
            Assert.AreEqual(79, DigitSubstring.FindByChunks("454"), "...454...");
            Assert.AreEqual(98, DigitSubstring.FindByChunks("455"), "...455...");
            //Assert.AreEqual(8, DigitSubstring.FindByChunks("910"), "...910...");
            //Assert.AreEqual(188, DigitSubstring.FindByChunks("9100"), "...9100...");
           // Assert.AreEqual(187, DigitSubstring.FindByChunks("99100"), "...99100...");
            Assert.AreEqual(190, DigitSubstring.FindByChunks("00101"), "...00101...");
            Assert.AreEqual(190, DigitSubstring.FindByChunks("001"), "...001...");
            //Assert.AreEqual(190, DigitSubstring.FindByChunks("00"), "...00...");
            Assert.AreEqual(0, DigitSubstring.FindByChunks("123456789"));
            Assert.AreEqual(0, DigitSubstring.FindByChunks("1234567891"));
            Assert.AreEqual(1000000071, DigitSubstring.FindByChunks("123456798"));
            Assert.AreEqual(9, DigitSubstring.FindByChunks("10"));
            Assert.AreEqual(13034, DigitSubstring.FindByChunks("53635"));
            Assert.AreEqual(1091, DigitSubstring.FindByChunks("040"));
            Assert.AreEqual(11, DigitSubstring.FindByChunks("11"));
            Assert.AreEqual(168, DigitSubstring.FindByChunks("99"));
            Assert.AreEqual(122, DigitSubstring.FindByChunks("667"));
            Assert.AreEqual(15050, DigitSubstring.FindByChunks("0404"));
            Assert.AreEqual(382689688L, DigitSubstring.FindByChunks("949225100")); //49225099->49225100
            Assert.AreEqual(24674951477L, DigitSubstring.FindByChunks("58257860625"));
            Assert.AreEqual(6957586376885L, DigitSubstring.FindByChunks("3999589058124"));
            Assert.AreEqual(1686722738828503L, DigitSubstring.FindByChunks("555899959741198"));
            Assert.AreEqual(10, DigitSubstring.FindByChunks("01"));
            Assert.AreEqual(170, DigitSubstring.FindByChunks("091"));
            Assert.AreEqual(2927, DigitSubstring.FindByChunks("0910"));
            Assert.AreEqual(2617, DigitSubstring.FindByChunks("0991"));
            Assert.AreEqual(2617, DigitSubstring.FindByChunks("09910"));
            Assert.AreEqual(35286, DigitSubstring.FindByChunks("09991"));
        }

        [Test]
        public void TestAlgoE()
        {
            Assert.AreEqual(139534109377208, DigitSubstring.FindByChunksE("02210760372892")); //10760372892022, d=14
            Assert.AreEqual(28824520244777, DigitSubstring.FindByChunksE("2923027408735")); //2302740873529->2302740873530, d=13
            Assert.AreEqual(8, DigitSubstring.FindByChunksE("910"));                          
            //Assert.AreEqual(185, DigitSubstring.FindByChunksE("9899100"));
            Assert.AreEqual(24674951477L, DigitSubstring.FindByChunksE("58257860625")); //2578606258->2578606259, d=10
            Assert.AreEqual(13034, DigitSubstring.FindByChunksE("53635"));
            Assert.AreEqual(1091, DigitSubstring.FindByChunksE("040"));
            Assert.AreEqual(168, DigitSubstring.FindByChunksE("99"));
            Assert.AreEqual(15050, DigitSubstring.FindByChunksE("0404"));
            Assert.AreEqual(6957586376885L, DigitSubstring.FindByChunksE("3999589058124")); //589058123999->589058124000
            Assert.AreEqual(1686722738828503L, DigitSubstring.FindByChunksE("555899959741198")); //119855589995974->119855589995975
            Assert.AreEqual(2927, DigitSubstring.FindByChunksE("0910"));
            Assert.AreEqual(2617, DigitSubstring.FindByChunksE("0991"));
            Assert.AreEqual(2617, DigitSubstring.FindByChunksE("09910")); //909->910
            Assert.AreEqual(35286, DigitSubstring.FindByChunksE("09991"));


            Assert.AreEqual(3, DigitSubstring.FindByChunksE("456"), "...456...");
            Assert.AreEqual(79, DigitSubstring.FindByChunksE("454"), "...454...");
            Assert.AreEqual(98, DigitSubstring.FindByChunksE("455"), "...455...");
            Assert.AreEqual(8, DigitSubstring.FindByChunksE("910"), "...910...");
            Assert.AreEqual(188, DigitSubstring.FindByChunksE("9100"), "...9100...");
            Assert.AreEqual(187, DigitSubstring.FindByChunksE("99100"), "...99100...");
            Assert.AreEqual(190, DigitSubstring.FindByChunksE("00101"), "...00101...");
            Assert.AreEqual(190, DigitSubstring.FindByChunksE("001"), "...001...");
            Assert.AreEqual(190, DigitSubstring.FindByChunksE("00"), "...00...");
            Assert.AreEqual(0, DigitSubstring.FindByChunksE("123456789"));
            Assert.AreEqual(0, DigitSubstring.FindByChunksE("1234567891"));
            Assert.AreEqual(1000000071, DigitSubstring.FindByChunksE("123456798"));
            Assert.AreEqual(9, DigitSubstring.FindByChunksE("10"));
            Assert.AreEqual(13034, DigitSubstring.FindByChunksE("53635"));
            Assert.AreEqual(1091, DigitSubstring.FindByChunksE("040"));
            Assert.AreEqual(11, DigitSubstring.FindByChunksE("11"));
            Assert.AreEqual(168, DigitSubstring.FindByChunksE("99"));
            Assert.AreEqual(122, DigitSubstring.FindByChunksE("667"));
            Assert.AreEqual(15050, DigitSubstring.FindByChunksE("0404"));
            Assert.AreEqual(382689688L, DigitSubstring.FindByChunksE("949225100")); //49225099->49225100, d=8
            Assert.AreEqual(24674951477L, DigitSubstring.FindByChunksE("58257860625"));
            Assert.AreEqual(6957586376885L, DigitSubstring.FindByChunksE("3999589058124"));
            Assert.AreEqual(1686722738828503L, DigitSubstring.FindByChunksE("555899959741198"));
            Assert.AreEqual(10, DigitSubstring.FindByChunksE("01"));
            Assert.AreEqual(170, DigitSubstring.FindByChunksE("091"));
            Assert.AreEqual(2927, DigitSubstring.FindByChunksE("0910"));
            Assert.AreEqual(2617, DigitSubstring.FindByChunksE("0991"));
            Assert.AreEqual(2617, DigitSubstring.FindByChunksE("09910"));
            Assert.AreEqual(35286, DigitSubstring.FindByChunksE("09991"));
        }

        [Test]
        public void TestAlgoInc()
        {
            Assert.AreEqual(16287543530, DigitSubstring.FindByInc("7398654641739")); //1 739 865 464->1 739 865 464, d=10 n=14
            Assert.AreEqual(2262734, DigitSubstring.FindByInc("039564")); //395640->395641, d=6 n=6
            Assert.AreEqual(64071, DigitSubstring.FindByInc("036150")); //15036->15037, d=5 n=6
            Assert.AreEqual(2143057660808, DigitSubstring.FindByInc("918784739766")); //187 847 397 659->187 847 397 660, d=12 n=13
            Assert.AreEqual(1956074471526, DigitSubstring.FindByInc("2191722654652")); //172 265 465 219->172 265 465 220, d=12 n=13
            Assert.AreEqual(28824520244777, DigitSubstring.FindByInc("2923027408735")); //2302740873529->2302740873530, d=13 n=13
            Assert.AreEqual(8, DigitSubstring.FindByInc("910"));
            //Assert.AreEqual(185, DigitSubstring.FindByInc("9899100"));
            Assert.AreEqual(24674951477L, DigitSubstring.FindByInc("58257860625")); //2578606258->2578606259, d=10
            Assert.AreEqual(13034, DigitSubstring.FindByInc("53635"));
            Assert.AreEqual(1091, DigitSubstring.FindByInc("040"));
            Assert.AreEqual(168, DigitSubstring.FindByInc("99"));
            Assert.AreEqual(15050, DigitSubstring.FindByInc("0404"));
            Assert.AreEqual(6957586376885L, DigitSubstring.FindByInc("3999589058124")); //589058123999->589058124000, d=12 n=13
            Assert.AreEqual(1686722738828503L, DigitSubstring.FindByInc("555899959741198")); //119855589995974->119855589995975
            Assert.AreEqual(214, DigitSubstring.FindByInc("0810"));
            Assert.AreEqual(2927, DigitSubstring.FindByInc("0910"));
            Assert.AreEqual(2617, DigitSubstring.FindByInc("0991"));
            Assert.AreEqual(2617, DigitSubstring.FindByInc("09910")); //909->910
            Assert.AreEqual(35286, DigitSubstring.FindByInc("09991")); //9099->9100, d=4 n=5


            Assert.AreEqual(3, DigitSubstring.FindByInc("456"), "...456...");
            Assert.AreEqual(79, DigitSubstring.FindByInc("454"), "...454...");
            Assert.AreEqual(98, DigitSubstring.FindByInc("455"), "...455...");
            Assert.AreEqual(8, DigitSubstring.FindByInc("910"), "...910...");
            Assert.AreEqual(188, DigitSubstring.FindByInc("9100"), "...9100...");
            Assert.AreEqual(187, DigitSubstring.FindByInc("99100"), "...99100...");
            Assert.AreEqual(190, DigitSubstring.FindByInc("00101"), "...00101...");
            Assert.AreEqual(190, DigitSubstring.FindByInc("001"), "...001...");
            Assert.AreEqual(190, DigitSubstring.FindByInc("00"), "...00...");
            Assert.AreEqual(0, DigitSubstring.FindByInc("123456789"));
            Assert.AreEqual(0, DigitSubstring.FindByInc("1234567891"));
            Assert.AreEqual(1000000071, DigitSubstring.FindByInc("123456798"));
            Assert.AreEqual(9, DigitSubstring.FindByInc("10"));
            Assert.AreEqual(13034, DigitSubstring.FindByInc("53635"));
            Assert.AreEqual(1091, DigitSubstring.FindByInc("040"));
            Assert.AreEqual(11, DigitSubstring.FindByInc("11"));
            Assert.AreEqual(168, DigitSubstring.FindByInc("99"));
            Assert.AreEqual(122, DigitSubstring.FindByInc("667"));
            Assert.AreEqual(15050, DigitSubstring.FindByInc("0404"));
            Assert.AreEqual(382689688L, DigitSubstring.FindByInc("949225100")); //49225099->49225100, d=8
            Assert.AreEqual(24674951477L, DigitSubstring.FindByInc("58257860625"));
            Assert.AreEqual(6957586376885L, DigitSubstring.FindByInc("3999589058124"));
            Assert.AreEqual(1686722738828503L, DigitSubstring.FindByInc("555899959741198"));
            Assert.AreEqual(10, DigitSubstring.FindByInc("01"));
            Assert.AreEqual(170, DigitSubstring.FindByInc("091"));
            Assert.AreEqual(2927, DigitSubstring.FindByInc("0910"));
            Assert.AreEqual(2617, DigitSubstring.FindByInc("0991"));
            Assert.AreEqual(2617, DigitSubstring.FindByInc("09910"));
            Assert.AreEqual(35286, DigitSubstring.FindByInc("09991")); 
        }

        [Test]
        public void TestLength()
        {
            Assert.AreEqual(24674951469L, DigitSubstring.GetLengthBefore(2578606258)); 
            Assert.AreEqual(8, DigitSubstring.GetLengthBefore(9));
            Assert.AreEqual(2889, DigitSubstring.GetLengthBefore(1000));
            Assert.AreEqual(9, DigitSubstring.GetLengthBefore(10));
            Assert.AreEqual(189, DigitSubstring.GetLengthBefore(100));
            Assert.AreEqual(11, DigitSubstring.GetLengthBefore(11));
            Assert.AreEqual(167, DigitSubstring.GetLengthBefore(89));
            Assert.AreEqual(627, DigitSubstring.GetLengthBefore(246));
            Assert.AreEqual(0, DigitSubstring.GetLengthBefore(1));
            Assert.AreEqual(28824520244779, DigitSubstring.GetLengthBefore(2302740873530));
        }

        [Test]
        public void TestGetNum()
        {
            Assert.AreEqual(11, DigitSubstring.GetNumAt(11));
            Assert.AreEqual(14, DigitSubstring.GetNumAt(17));
            Assert.AreEqual(8, DigitSubstring.GetNumAt(7));
            Assert.AreEqual(1, DigitSubstring.GetNumAt(0));
        }
    }
}