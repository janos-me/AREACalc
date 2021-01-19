using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AREACalc
{
    public class Program
    {
        public static PointF center = new PointF();
        static void Main()  
        {
            //  Non-console input---------------------------------------------------------------------begin
            //  ><((((o>

            //var K = 0;
            //var rawVertices = "0 0 0 0";
            //var N = 0;
            //string rawLinObj 
            //    "0 0 0 0 " +
            //    "0 0 0 0 ";

            //var resultArea = NonConsoleInputResult(K, N, rawVertices, rawLinObj);
            //Console.WriteLine("The area of the land plot under the linear objects is { 0 }.", resultArea);

            //  ><((((o>
            //  Non-console input---------------------------------------------------------------------end

//--------------------------------------------------------------------------------------------------------------------------------------------------------

            //  Console input---------------------------------------------------------------------------begin
            //   (\__ /)
            //  (= '.' =)
            //   (")_(") 

            Console.WriteLine("Enter the number of pairs of coordinates (x;y) of the vertices.");
            //  The number of vertices
            var K = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Enter the number of linear objects.");
            //  The number of the linear objects
            var N = Int32.Parse(Console.ReadLine());
            
            if (CheckKN(K, N))
            {
                Console.WriteLine("Enter pairs of coordinates (x;y) of the vertices.\nInput format: x1 y1 x2 y2 ... xk yk");
                var rawVertices = Console.ReadLine();

                if (CheckVerticesPairs(rawVertices, K))
                {
                    var linearObjectsArray = new float[N, 4];

                    for (var numberOfLinObj = 0; numberOfLinObj < N; numberOfLinObj++)
                    {
                        Console.WriteLine("Enter parameters for {0} linear object.", numberOfLinObj + 1);
                        var linearObjectString = Console.ReadLine();

                        if (CheckLinearObjectParameters(linearObjectString))
                            LinearObjectsConversionFromConsole(linearObjectString, numberOfLinObj, linearObjectsArray);
                        else
                        {
                            Console.WriteLine("Incorrect data.\nInput format for the parameters is: w A B C, w > 0.");
                            numberOfLinObj--;
                        }
                    }
                    var enteredPolygon = VerticesCoversion(rawVertices, K);
                    var mainArea = FindAreaOfPolygon(enteredPolygon);
                    if (CoverAllCheck(linearObjectsArray, enteredPolygon, N))
                    {
                        Console.WriteLine("The area of the land plot under the linear objects is {0}.", (int)Math.Round(mainArea));
                    }
                    else
                    {
                        var referencePolygonList = CreateReferencePolygonList(enteredPolygon, linearObjectsArray, N);
                        var resultArea = FindAreaUnderLinearObjects(mainArea, referencePolygonList);
                        Console.WriteLine("The area of the land plot under the linear objects is {0}.", resultArea);
                    }
                }
                else
                    Console.WriteLine("Incorrect number of the vertices.");
            }
            else Console.WriteLine("Wrong number of vertices or linear objects.");

            //   (\__ /)
            //  (= '.' =)
            //   (")_(")
            //  Console input---------------------------------------------------------------------------end

            Console.ReadKey();
        }
        /// <summary>
        /// Checks the entered parameters.
        /// </summary>
        /// <param name="K"></param>
        /// <param name="N"></param>
        /// <returns>Bool result.</returns>
        public static bool CheckKN(int K, int N)
        {
            if (K >= 1 && K <= 50 && N >= 1 && N <= 100)
                return true;
            else return false;
        }
        /// <summary>
        /// Sorts the polygon vertices in the clockwise order.
        /// </summary>
        /// <param name="convexPolygon"></param>
        /// <returns>A sorted polygon.</returns>
        public static PointF[] GetSortedPolygon(PointF[] convexPolygon)
        {
            center = GetCenterPoint(convexPolygon);
            Array.Sort(convexPolygon, new ClockwiseComparer());
            Array.Reverse(convexPolygon);
            Array.Resize(ref convexPolygon, convexPolygon.Length + 1);
            convexPolygon[convexPolygon.Length - 1] = new PointF(convexPolygon[0].X, convexPolygon[0].Y);

            return convexPolygon;
        }
        /// <summary>
        /// Gets the result from non-console input. Alternative program start.
        /// </summary>
        /// <param name="K"></param>
        /// <param name="N"></param>
        /// <param name="rawVertices"></param>
        /// <param name="rawLinObj"></param>
        /// <returns>An area of the land plot under the linear objects.</returns>
        public static int NonConsoleInputResult(int K, int N, string rawVertices, string rawLinObj)
        {
            var enteredPolygon = VerticesCoversion(rawVertices, K);
            var linearObjectsArray = LinearObjectsConversion(rawLinObj, N);
            var mainArea = FindAreaOfPolygon(enteredPolygon);
            if (CoverAllCheck(linearObjectsArray, enteredPolygon, N))
            {
                return (int)Math.Round(mainArea);
            }
            else
            {
            var referencePolygonList = CreateReferencePolygonList(enteredPolygon, linearObjectsArray, N);

            var resultArea = FindAreaUnderLinearObjects(mainArea, referencePolygonList);
            return resultArea;
            }
        }
        /// <summary>
        /// Gets all polygons that are not under the linear objects.
        /// </summary>
        /// <param name="enteredPolygon"></param>
        /// <param name="linearObjectsArray"></param>
        /// <param name="N"></param>
        /// <returns>A list of polygons.</returns>
        public static List<PointF[]> CreateReferencePolygonList(PointF[] enteredPolygon, float[,] linearObjectsArray, int N)
        {
            var polygonFlag = 1;
            var polygonsList1 = new List<PointF[]>();
            var polygonsList2 = new List<PointF[]>();
            polygonsList1.Add(enteredPolygon);

            var referencePolList = polygonsList1;

            for (var numberOfLinObj = 0; numberOfLinObj < N; numberOfLinObj++)
            {
                var w = linearObjectsArray[numberOfLinObj, 0];
                var A = linearObjectsArray[numberOfLinObj, 1];
                var B = linearObjectsArray[numberOfLinObj, 2];
                var C = linearObjectsArray[numberOfLinObj, 3];
                var CPlus = C + w / 2;
                var CMinus = C - w / 2;

                if (polygonFlag == 1 && w > 0)
                {
                    polygonsList2.Clear();
                    polygonsList2 = CreateListOfNotUnderTheLinObjPolygonsForOneLinObj(polygonsList1, A, B, CPlus, CMinus);
                    polygonFlag = 2;
                    referencePolList = polygonsList2;
                }
                else if (polygonFlag == 2 && w > 0)
                {
                    polygonsList1.Clear();
                    polygonsList1 = CreateListOfNotUnderTheLinObjPolygonsForOneLinObj(polygonsList2, A, B, CPlus, CMinus);
                    polygonFlag = 1;
                    referencePolList = polygonsList1;
                }
            }
            return referencePolList;
        }
        /// <summary>
        /// Finds an area of the land plot under the linear objects by substracting not-under-the-linear-objects-polygons-area from entered polygon's area.
        /// </summary>
        /// <param name="mainArea"></param>
        /// <param name="referencePolygonList"></param>
        /// <returns>An area of the land plot under the linear objects.</returns>
        public static int FindAreaUnderLinearObjects(double mainArea, List<PointF[]> referencePolygonList)
        {
            var sumPolygonsArea = 0.0;
            for (var i = 0; i < referencePolygonList.Count; i++)
            {
                sumPolygonsArea += FindAreaOfPolygon(referencePolygonList[i]);
            }
            var areaUnderLinObj = mainArea - sumPolygonsArea;
            areaUnderLinObj = Math.Round(areaUnderLinObj);
            return (int)areaUnderLinObj;
        }
        /// <summary>
        /// Gets a list of polygons that not under the linear object.
        /// </summary>
        /// <param name="polygonsList"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="CPlus"></param>
        /// <param name="CMinus"></param>
        /// <returns>A list of polygons.</returns>
        public static List<PointF[]> CreateListOfNotUnderTheLinObjPolygonsForOneLinObj(List<PointF[]> polygonsList, float A, 
            float B, float CPlus, float CMinus)
        {
            var newPolygonsList = new List<PointF[]>();

            for (var polygon = 0; polygon < polygonsList.Count; polygon++)
            {
                var currentPolygon = polygonsList[polygon];

                bool isPlus = true;
                bool polygonHasIntersections = false;
                var intersections = FindIntersectionPoints(A, B, CPlus, currentPolygon);

                if (intersections.Count == 2)
                {
                    newPolygonsList.Add(CreateNewPolygon(intersections, currentPolygon, A, B, CPlus, isPlus));
                    polygonHasIntersections = true;
                }

                isPlus = false;
                intersections = FindIntersectionPoints(A, B, CMinus, currentPolygon);

                if (intersections.Count == 2)
                {
                    newPolygonsList.Add(CreateNewPolygon(intersections, currentPolygon, A, B, CMinus, isPlus));
                    polygonHasIntersections = true;
                }

                if (polygonHasIntersections == false && !CoverPolygon(A, B, CPlus, CMinus, currentPolygon)) newPolygonsList.Add(currentPolygon);
            }
                return newPolygonsList;
        }
        public static bool CoverPolygon(float A, float B, float CPlus, float CMinus, PointF[] convexPolygon)
        {
            bool coverPolygon = true;
                for (var point = 0; point < convexPolygon.Length - 1; point++)
                {
                    var x = convexPolygon[point].X;
                    var y = convexPolygon[point].Y;

                    var resultPlus = A * x + B * y + CPlus;
                    var resultMinus = A * x + B * y + CMinus;

                    if (resultPlus < 0 || resultMinus > 0)
                    {
                        coverPolygon = false;
                        break;
                    }
                }
            return coverPolygon;
        }
        /// <summary>
        /// Finds an area of the polygon.
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns>The area of the polygon</returns>
        public static double FindAreaOfPolygon(PointF[] polygon)
        {
            var positiveHalf = 0.0;
            var negativeHalf = 0.0;

            for (var elemInPolygon = 0; elemInPolygon < polygon.Length-1; elemInPolygon++)
            {
                positiveHalf += polygon[elemInPolygon].X * polygon[elemInPolygon+1].Y;
                negativeHalf += polygon[elemInPolygon + 1].X * polygon[elemInPolygon].Y;
            }
            var area = (positiveHalf - negativeHalf) / 2;
            area = Math.Abs(area);
            return area;
        }

        /// <summary>
        /// Gets a center point of the polygon.
        /// </summary>
        /// <param name="points"></param>
        /// <returns>The center point of the polygon.</returns>
        public static PointF GetCenterPoint(PointF[] points)
        {
            var sumx = 0.0f;
            var sumy = 0.0f;

            var centerPoint = new PointF();
            for (var i = 0; i < points.Length; i++)
            {
                var x = points[i].X;
                var y = points[i].Y;

                sumx += x;
                sumy += y;
            }
            centerPoint.X = sumx / points.Length;
            centerPoint.Y = sumy / points.Length;

            return centerPoint;
        }
        /// <summary>
        /// Creates a new polygon clipped by the specified straight line with the parameters A, B, C.
        /// </summary>
        /// <param name="intersectionPoints"></param>
        /// <param name="polygon"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="plus"></param>
        /// <returns></returns>
        public static PointF[] CreateNewPolygon(List<PointF> intersectionPoints, PointF[] polygon, float A, float B, float C, bool plus)
        {
            var newPolygon = new List<PointF>();
            newPolygon.Add(new PointF(intersectionPoints[0].X, intersectionPoints[0].Y));
            newPolygon.Add(new PointF(intersectionPoints[1].X, intersectionPoints[1].Y));
            for (var point = 0; point < polygon.Length-1; point++)
            {
                var x = polygon[point].X;
                var y = polygon[point].Y;

                var result = A * x + B * y + C;

                if (plus == true && result < 0)
                    newPolygon.Add(new PointF(x, y));
                else
                if (plus == false && result > 0) 
                    newPolygon.Add(new PointF(x, y));
            }
            var newPolygonArr = newPolygon.ToArray();
            center = GetCenterPoint(newPolygonArr);
            newPolygonArr = GetSortedPolygon(newPolygonArr);

            return newPolygonArr;
        }
        /// <summary>
        /// Searches for any of the linear objects that could cover the entire entered polygon.
        /// </summary>
        /// <param name="linearObjectsArray"></param>
        /// <param name="convexPolygon"></param>
        /// <param name="N"></param>
        /// <returns>Bool result.</returns>
        public static bool CoverAllCheck(float[,] linearObjectsArray, PointF[] convexPolygon, int N)
        {
            bool coverAllVertices = false;
            for (var numberOfLinObj = 0; numberOfLinObj < N; numberOfLinObj++)
            {
                coverAllVertices = true;
                var w = linearObjectsArray[numberOfLinObj, 0];
                var A = linearObjectsArray[numberOfLinObj, 1];
                var B = linearObjectsArray[numberOfLinObj, 2];
                var C = linearObjectsArray[numberOfLinObj, 3];
                var CPlus = C + w / 2;
                var CMinus = C - w / 2;
                for (var point = 0; point < convexPolygon.Length-1; point++)
                {
                    var x = convexPolygon[point].X;
                    var y = convexPolygon[point].Y;

                    var resultPlus = A * x + B * y + CPlus;
                    var resultMinus = A * x + B * y + CMinus;

                    if (resultPlus < 0 || resultMinus > 0)
                    {
                        coverAllVertices = false;
                        break;
                    }
                }
                if (coverAllVertices) break;
            }
            return coverAllVertices;
        }
        /// <summary>
        /// Gets the parameters A, B, C for composing the linear equation.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="y1"></param>
        /// <param name="y2"></param>
        /// <returns>An array of float with parameters.</returns>
        public static float[] GetParamsFrom2PairsOfCoordinates(float x1, float x2, float y1, float y2)
        {
            var paramArray = new float[3];
            paramArray[0] = y2 - y1;
            paramArray[1] = -(x2 - x1);
            paramArray[2] = -(x1 * (y2 - y1) - y1 * (x2 - x1));

            return paramArray;
        }
        /// <summary>
        /// Finds an intersection points of a straight line with the sides of a polygon.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="polygon"></param>
        /// <returns>List of intersection points.</returns>
        public static List<PointF> FindIntersectionPoints(float A, float B, float C, PointF[] polygon)
        {
            var intersectionPointsList = new List<PointF>();
            for (int i = 0; i < polygon.Length-1; i++)
            {
                var x1p = polygon[i].X;
                var y1p = polygon[i].Y;
                var x2p = polygon[i+1].X;
                var y2p = polygon[i+1].Y;

                var verticesParams = GetParamsFrom2PairsOfCoordinates(x1p, x2p, y1p, y2p);
                var A1 = verticesParams[0];
                var B1 = verticesParams[1];
                var C1 = verticesParams[2];

                var x = -((C * B1 - C1 * B) / (A * B1 - A1 * B));
                var y = -((A * C1 - A1 * C) / (A * B1 - A1 * B));

                if (CheckIntersectionsOfPointAndSection(x, y, x1p, x2p, y1p, y2p))
                {
                    intersectionPointsList.Add(new PointF(x, y));
                }
            }
            return intersectionPointsList.Distinct().ToList();
        }
        /// <summary>
        /// Checks whether the point is located on the specified section.
        /// </summary>
        /// <param name="xPoint"></param>
        /// <param name="yPoint"></param>
        /// <param name="x1Sec"></param>
        /// <param name="x2Sec"></param>
        /// <param name="y1Sec"></param>
        /// <param name="y2Sec"></param>
        /// <returns>Bool result.</returns>
        public static bool CheckIntersectionsOfPointAndSection(float xPoint, float yPoint, float x1Sec, 
            float x2Sec, float y1Sec, float y2Sec)
        {
            if ((xPoint == x1Sec && yPoint == y1Sec) || (xPoint == x2Sec && yPoint == y2Sec)) return true;
            else
            {
            var result = (xPoint - x1Sec) * (y2Sec - y1Sec) - (yPoint - y1Sec) * (x2Sec - x1Sec);
                result = (float)Math.Round(result, 3);
            if (result == 0 && ((x1Sec<xPoint && xPoint<x2Sec) || (x2Sec<xPoint && xPoint<x1Sec) ||
                (y1Sec < yPoint && yPoint < y2Sec) || (y2Sec < yPoint && yPoint < y1Sec))) return true;
            else return false;
            }
        }
        /// <summary>
        /// For non-console input. Converts string of the parameters for linear objects (each must be separated by a space) to a two-dimensional array of float.
        /// </summary>
        /// <param name="linObjStr"></param>
        /// <param name="N"></param>
        /// <returns>Two-dimensional array of float that contains parameters.</returns>
        public static float[,] LinearObjectsConversion(string linObjStr, int N)
        {
            var linObjParams = new float[N, 4];
            var stringCounterForLinObjArr = 0;
            var linObjParamsString = linObjStr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //  If the width < 1 => the linear object's area = 0
            for (var param = 0; param < linObjParamsString.Length; param+=4)
            {
                if (float.Parse(linObjParamsString[param]) > 0)
                {
                    linObjParams[stringCounterForLinObjArr, 0] = float.Parse(linObjParamsString[param]);
                    linObjParams[stringCounterForLinObjArr, 1] = float.Parse(linObjParamsString[param + 1]);
                    linObjParams[stringCounterForLinObjArr, 2] = float.Parse(linObjParamsString[param + 2]);
                    linObjParams[stringCounterForLinObjArr, 3] = float.Parse(linObjParamsString[param + 3]);
                    stringCounterForLinObjArr++;
                }
            }
            if (stringCounterForLinObjArr != N)
            {
                var array = new float[stringCounterForLinObjArr, 4];
                Array.Copy(linObjParams, array, stringCounterForLinObjArr * 4);
                return array;
            }
            else
            return linObjParams;
        }
        /// <summary>
        /// Converts a parameter string to an array of float.
        /// </summary>
        /// <param name="linObjStr"></param>
        /// <param name="index"></param>
        /// <param name="paramsArray"></param>
        public static void LinearObjectsConversionFromConsole(string linObjStr, int index, float[,] paramsArray)
        {
            var linObjParams = linObjStr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (linObjParams.Length == 4)
            {
                paramsArray[index, 0] = float.Parse(linObjParams[0]);
                paramsArray[index, 1] = float.Parse(linObjParams[1]);
                paramsArray[index, 2] = float.Parse(linObjParams[2]);
                paramsArray[index, 3] = float.Parse(linObjParams[3]);
            }
        }
        /// <summary>
        /// Checks that the entered linear object has 4 parameters.
        /// </summary>
        /// <param name="rawParameters"></param>
        /// <returns>Bool result.</returns>
        public static bool CheckLinearObjectParameters(string rawParameters)
        {
            var parameters = rawParameters.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var w = float.Parse(parameters[0]);
            if (parameters.Length == 4 && w > 0) return true;
            else return false;
        }

        /// <summary>
        /// Checks the entered string of vertices. It must be even and equal to the previously entered K.
        /// </summary>
        /// <param name="rawVertices"></param>
        /// <param name="numberOfVertices"></param>
        /// <returns>Bool result.</returns>
        public static bool CheckVerticesPairs(string rawVertices, int numberOfVertices)
        {
            var vertices = rawVertices.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var enteredNumberOfVertices = vertices.Length / 2;

            if (vertices.Length % 2 != 0 || enteredNumberOfVertices != numberOfVertices) return false;
            else return true;
        }

        /// <summary>
        /// Converts a string of vertices to an array of PointF[].
        /// </summary>
        /// <param name="rawVertices"></param>
        /// <param name="points"></param>
        public static PointF[] VerticesCoversion(string rawVertices, int K)
        {
            var points = new PointF[K];
            var vertices = rawVertices.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
            var counterForPointsIndex = 0;

            for (var i = 0; i < vertices.Length; i += 2)
            {
                var x = float.Parse(vertices[i]);
                var y = float.Parse(vertices[i+1]);
                points[counterForPointsIndex] = new PointF(x, y);
                counterForPointsIndex++;
            }
            points = GetSortedPolygon(points);

            return points;
        }
    }
}