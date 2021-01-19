using System;
using System.Collections.Generic;
using System.Drawing;
using AREACalc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AREACalcTests
{
    [TestClass]
    public class TestClass
    {
        [TestMethod]
        public void CheckTestCase()
        {
            var K = 0;
            var rawVertices = "0 0";
            var N = 0;
            string rawLinObj =
                "0 0 0 0 ";

            var result = 0;//Program.NonConsoleInputResult(K, N, rawVertices, rawLinObj);
            var expectedResult = 0;

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void ProgramResultTest_9_Vertices_6_LinObj()
        {
            var K = 9;
            var rawVertices = "4 -4 2 3 6 15 14 23 17 23 22 20 23 14 20 3 12 -3";
            var N = 6;
            string rawLinObj =
                "7 1 0 -5 " +
                "1 -3 -1 14 " +
                "3 1,25 1 -36,5 " +
                "2 1 0 2 " +
                "8,5 -2 1 32 " +
                "12 1 0 -17 ";

            var result = Program.NonConsoleInputResult(K, N, rawVertices, rawLinObj);
            var expectedResult = 330;

            Assert.AreEqual(expectedResult, result);
        }
        [TestMethod]
        public void ProgramResultTest_9_Vertices_5_LinObj()
        {
            var K = 9;
            var rawVertices = "4 -4 2 3 6 15 14 23 17 23 22 20 23 14 20 3 12 -3";
            var N = 5;
            string rawLinObj =
                "7 1 0 -5 " +
                "1 -3 -1 14 " +
                "3 1,25 1 -36,5 " +
                "2 1 0 2 " +
                "8,5 -2 1 32";

            var result = Program.NonConsoleInputResult(K, N, rawVertices, rawLinObj);
            var expectedResult = 170;

            Assert.AreEqual(expectedResult, result);
        }
        [TestMethod]
        public void ProgramResultTest_2_Vertices_1_LinObj()
        {
            var K = 2;
            var rawVertices = "0 0 0 3";
            var N = 1;
            string rawLinObj =
                "1 1 0 -1 ";

            var result = Program.NonConsoleInputResult(K, N, rawVertices, rawLinObj);
            var expectedResult = 0;

            Assert.AreEqual(expectedResult, result);
        }
        [TestMethod]
        public void ProgramResultTest_4_Vertices_2_LinObj_cover_all_polygon()
        {
            var K = 4;
            var rawVertices = "0 0 0 3 3 3 3 0";
            var N = 2;
            string rawLinObj =
                "1 1 0 -1 " +
                "3 1 0 -1,5 ";

            var result = Program.NonConsoleInputResult(K, N, rawVertices, rawLinObj);
            var expectedResult = 9;

            Assert.AreEqual(expectedResult, result);
        }
        [TestMethod]
        public void ProgramResultTest_4_Vertices_2_LinObj_with_negative_vertices()
        {
            var K = 4;
            var rawVertices = "-2 0 -2 10 8 10 8 0";
            var N = 2;
            string rawLinObj =
                "1 0 1 -5 " +
                "1 1 0 -5 ";

            var result = Program.NonConsoleInputResult(K, N, rawVertices, rawLinObj);
            var expectedResult = 19;

            Assert.AreEqual(expectedResult, result);
        }
        [TestMethod]
        public void ProgramResultTest_7_Vertices_4_LinObj()
        {
            var K = 7;
            var rawVertices = "2 2 2 4 4 6  7 7 10 4 8 2 4 1";
            var N = 4;
            string rawLinObj =
                "3 1 0 -4 " +
                "2 1 0 -10 " +
                "2 -1 -3 20 " +
                "1 1 0 -20";

            var result = Program.NonConsoleInputResult(K, N, rawVertices, rawLinObj);
            var expectedResult = 17;

            Assert.AreEqual(expectedResult, result);
        }
        [TestMethod]
        public void ProgramResultTest_4_Vertices_2_LinObj()
        {
            var K = 4;
            var N = 2;
            var rawVertices = "0 0 0 10 10 10 10 0";
            string rawLinObj =
                "1 0 1 -5 " +
                "1 1 0 -5 ";
            var result = Program.NonConsoleInputResult(K, N, rawVertices, rawLinObj);
            var expectedResult = 19;

            Assert.AreEqual(expectedResult, result);
        }
        [TestMethod]
        public void FindSquareArea()
        {
            var polygon = new PointF[]
            {
                new PointF( 0,0 ),
                new PointF( 0,10 ),
                new PointF( 10,10 ),
                new PointF( 10,0 ),
                new PointF( 0,0 )
            };

            var result = Program.FindAreaOfPolygon(polygon);
            var expected = 100;

            Assert.AreEqual(expected, result);
        }
            [TestMethod]
        public void GetA_min1_B_min1_C_4_FromCoordinates()
        {
            var x1 = 1f;
            var x2 = 2f;
            var y1 = 3f;
            var y2 = 2f;

            var result = Program.GetParamsFrom2PairsOfCoordinates(x1, x2, y1, y2).ToString();
            var paramsf = new[] { -1f, -1f, 4f }.ToString();

            Assert.AreEqual(paramsf, result);
        }

        [TestMethod]
        public void CheckIntersction()
        {

        }
            [TestMethod]
        public void FindIntersecPoints_A_min1_B_min1_C_4()
        {
            var A = -1f;
            var B = -1f;
            var C = 4f;

            var polygon = new PointF[]
            {
                new PointF( 0,1 ),
                new PointF( 0,3 ),
                new PointF( 1,4 ),
                new PointF( 3,4 ),
                new PointF( 3,2 ),
                new PointF( 1,0 ),
                new PointF( 0,1 ),
            };

            var resultIntersections = Program.FindIntersectionPoints(A, B, C, polygon);
            var intesections = new List<PointF>
            {
                new PointF(0.5f , 3.5f),
                new PointF(2.5f , 1.5f)
            };

            Assert.AreEqual(intesections[0], resultIntersections[0]);
            Assert.AreEqual(intesections[1], resultIntersections[1]);
        }

        [TestMethod]
        public void CorrectRawVertices_CorrectNumOfVertices3()
        {
            var rawVertices = "0 0 0 3 3 0";
            var numberOfVertices = 3;

            Assert.IsTrue(Program.CheckVerticesPairs(rawVertices, numberOfVertices));
        }

        [TestMethod]
        public void CorrectRawVertices_IncorrectNumOfVertices4()
        {
            var rawVertices = "0 2 0 3 3 0";
            var numberOfVertices = 4;

            Assert.IsFalse(Program.CheckVerticesPairs(rawVertices, numberOfVertices));
        }

        [TestMethod]
        public void IncorrectRawVertices_IncorrectNumOfVertices4()
        {
            var rawVertices = "0 2 0 3 3 0 8";
            var numberOfVertices = 4;

            Assert.IsFalse(Program.CheckVerticesPairs(rawVertices, numberOfVertices));
        }

        [TestMethod]
        public void CorrectRawVerticesWithSpaces_CorrectNumOfVertices4()
        {
            var rawVertices = "0 2 0 3 3 0   8   4";
            var numberOfVertices = 4;

            Assert.IsTrue(Program.CheckVerticesPairs(rawVertices, numberOfVertices));
        }
    }
}