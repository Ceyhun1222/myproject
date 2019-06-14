using ESRI.ArcGIS.Geometry;

namespace Aran.PANDA.Departure
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public struct TIntersections
	{
		public ESRI.ArcGIS.Geometry.IPoint pPoint;
		public int IntersectIndex;
		public int PointIndex;
		public int PredIndex;
		public double Dist;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct TPointsHeap
	{
		public ESRI.ArcGIS.Geometry.IPoint pPoint;
		public bool IsIntersection;
		public int IntersectIndex;
		public int UsedFlag;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public static class PolyCut
	{
		public static IPolygon MissingPolygon = null;

		private const double Epsilon = 0.0000000001;

		public static int ClipByLine(IPolygon pClipPolygon, IPolyline pCutLine, out IPolygon pLeft, out IPolygon pRight, out IPolygon pUnspecified)
		{
			pLeft = (IPolygon)(new Polygon());
			pRight = (IPolygon)(new Polygon());
			pUnspecified = (IPolygon)(new Polygon());

			if (pClipPolygon.IsEmpty)
				return 0;

			ESRI.ArcGIS.esriSystem.IClone pClone = (ESRI.ArcGIS.esriSystem.IClone)pClipPolygon;

			IPolygon4 pPolygon = (IPolygon4)pClone.Clone();

			ITopologicalOperator2 pTopoOper = (ITopologicalOperator2)pPolygon;
			pTopoOper.IsKnownSimple_2 = false;
			pTopoOper.Simplify();

			IGeometryCollection pIUnspec = (IGeometryCollection)pUnspecified;
			IGeometryCollection pTestPoly = (IGeometryCollection)(new Polygon());

			IGeometryBag pExteriorRings = pPolygon.ExteriorRingBag;

			IPoint ptFrom = pCutLine.FromPoint;
			IPoint ptTo = pCutLine.ToPoint;

			double dXl = ptTo.X - ptFrom.X;
			double dYl = ptTo.Y - ptFrom.Y;

			IEnumGeometry exteriorRingsEnum = (IEnumGeometry)pExteriorRings;
			exteriorRingsEnum.Reset();

			int result = 0;
			IRing pRing = (IRing)exteriorRingsEnum.Next();
			while (pRing != null)
			{
				IProximityOperator pProxy;

				if (pRing.IsExterior)
					pProxy = (IProximityOperator)ptFrom;
				else
					pProxy = (IProximityOperator)ptTo;

				IPointCollection pRingVertices = (IPointCollection)pRing;

				int n = pRingVertices.PointCount - 1;
				if (n >= 0)
				{
					int Side = 0, l = -1;
					TIntersections[] pIntersectionPoints = new TIntersections[n];
					
					do
					{
						l++;
						if (l >= n)
							break;

						Side = WhichSide(pCutLine, pRingVertices.get_Point(l));
					}
					while (Side == 0);

					int cnt = 0;
					// ' GetIntersectPointOfTwoLines
					for (int j = 0; j <= n - 1; j++)
					{
						int k = (l + 1) % n;
						IPoint pt1 = pRingVertices.get_Point(k);

						int Side1 = WhichSide(pCutLine, pt1);
						if (Side1 * Side < 0)
						{
							IPoint pt0 = pRingVertices.get_Point(l);

							Side = Side1;
							double dXs = pt1.X - pt0.X;
							double dYs = pt1.Y - pt0.Y;
							double down = dYl * dXs - dXl * dYs;

							if (System.Math.Abs(down) > Epsilon)
							{
								double dX = pt0.X - ptFrom.X;
								double dY = pt0.Y - ptFrom.Y;

								double UpA = dXl * dY - dYl * dX;
								double UpB = dXs * dY - dYs * dX;

								double mA = UpA / down;
								double mB = UpB / down;

								if ((mA >= 0.0 && mA < 1.0) && (mB >= 0.0 && mB < 1.0))
								{
									IPoint ptX = new ESRI.ArcGIS.Geometry.Point();
									ptX.PutCoords(pt0.X + mA * dXs, pt0.Y + mA * dYs);

									pIntersectionPoints[cnt].pPoint = ptX;
									pIntersectionPoints[cnt].Dist = pProxy.ReturnDistance(ptX);
									pIntersectionPoints[cnt].PredIndex = l;
									cnt++;
								}
							}
						}
						l = k;
					}

					if (cnt > 0)
					{
						pTestPoly.RemoveGeometries(0, pTestPoly.GeometryCount);
						pTestPoly.AddGeometry(pRing);

						pProxy = ((ESRI.ArcGIS.Geometry.IProximityOperator)(pTestPoly));
						int UBd;

						if (pProxy.ReturnDistance(ptTo) != 0.0)
							UBd = cnt - 1;
						else
							UBd = cnt - 2;

						if (pProxy.ReturnDistance(ptFrom) == 0.0)
						{
							pIntersectionPoints[0] = pIntersectionPoints[UBd];
							UBd = UBd - 1;
						}

						if (UBd > 0)
						{
							System.Array.Resize<TIntersections>(ref pIntersectionPoints, UBd + 1);
							TPointsHeap[] pPointsHeap = new TPointsHeap[n + UBd + 1];

							ArrangeVertices(pIntersectionPoints, pRingVertices, pPointsHeap);
							FormPolygons(pIntersectionPoints, pPointsHeap, pCutLine, pLeft, pRight, pUnspecified);

							result = 3;
						}
						pProxy = ((ESRI.ArcGIS.Geometry.IProximityOperator)(ptFrom));
					}
					else
						pIUnspec.AddGeometry(pRing);
				}

				pRing = ((ESRI.ArcGIS.Geometry.IRing)(exteriorRingsEnum.Next()));
			}



			if (result != 0)
			{
				IPointCollection pFormedPolygon = (IPointCollection)pLeft;
				if ((pFormedPolygon.PointCount < 3) && (pFormedPolygon.PointCount > 0))
					pFormedPolygon.RemovePoints(0, pFormedPolygon.PointCount);

				IArea pArea = (IArea)pLeft;
				if (pArea.Area >= 0.5)
				{
					pTopoOper = (ITopologicalOperator2)pLeft;
					pTopoOper.IsKnownSimple_2 = false;
					pTopoOper.Simplify();
				}
				else
					pLeft = (IPolygon)new Polygon();

				pFormedPolygon = (IPointCollection)pUnspecified;
				if ((pFormedPolygon.PointCount < 3) && (pFormedPolygon.PointCount > 0))
					pFormedPolygon.RemovePoints(0, pFormedPolygon.PointCount);

				pArea = (IArea)pUnspecified;
				if (pArea.Area >= 0.5)
				{
					pTopoOper = (ITopologicalOperator2)pUnspecified;
					pTopoOper.IsKnownSimple_2 = false;
					pTopoOper.Simplify();
				}
				else
					pUnspecified = (IPolygon)(new Polygon());

				pFormedPolygon = (IPointCollection)pRight;
				if ((pFormedPolygon.PointCount < 3) && (pFormedPolygon.PointCount > 0))
					pFormedPolygon.RemovePoints(0, pFormedPolygon.PointCount);

				pArea = (IArea)pRight;
				if (pArea.Area >= 0.5)
				{
					pTopoOper = (ITopologicalOperator2)pRight;
					pTopoOper.IsKnownSimple_2 = false;
					pTopoOper.Simplify();
				}
				else
					pRight = (IPolygon)(new Polygon());
			}

			return result;
		}

		public static int ClipByLine(IPointCollection pClipPolygon, IPolyline pCutLine, out IPolygon pLeft, out IPolygon pRight, out IPolygon pUnspecified)
		{
			return ClipByLine((IPolygon)pClipPolygon, pCutLine, out pLeft, out pRight, out pUnspecified);
		}

		public static int ClipByLine(IPointCollection pClipPolygon, IPolyline pCutLine, out IPointCollection pLeft, out IPointCollection pRight, out IPointCollection pUnspecified)
		{
			IPolygon pL, pR, pU;
			int r = ClipByLine((IPolygon)pClipPolygon, pCutLine, out pL, out pR, out pU);
			pLeft = pL as IPointCollection;
			pRight = pR as IPointCollection;
			pUnspecified = pU as IPointCollection;
			return r;
		}

		public static IPolyline ClipByPoly(IPolyline pClipLine, IPolygon pCutPolygon)
		{
			ESRI.ArcGIS.Geometry.IPolyline clipByPolyReturn = null;
			int iRings = 0;
			int Side1 = 0;
			int Side = 0;
			int Cnt = 0;
			int UBd = 0;
			int I = 0;
			int J = 0;
			int K = 0;
			int L = 0;
			int N = 0;

			double Down = 0;
			double UpA = 0;
			double UpB = 0;
			double mA = 0;
			double mB = 0;

			double dXl = 0;
			double dYl = 0;
			double dXs = 0;
			double dYs = 0;
			double dX = 0;
			double dY = 0;

			TIntersections[] pIntersectionPoints = null;
			ESRI.ArcGIS.Geometry.IGeometryCollection pGeomCol = null;

			ESRI.ArcGIS.Geometry.ITopologicalOperator2 pTopoOper = null;
			ESRI.ArcGIS.Geometry.IPointCollection pRingVertices = null;
			ESRI.ArcGIS.Geometry.IProximityOperator pProxy = null;
			ESRI.ArcGIS.Geometry.IPolygon2 pPolygon = null;
			ESRI.ArcGIS.Geometry.IPolyline pCutLine = null;
			ESRI.ArcGIS.Geometry.IPath pPline = null;
			ESRI.ArcGIS.esriSystem.IClone pClone = null;
			ESRI.ArcGIS.Geometry.IRing pRing = null;

			ESRI.ArcGIS.Geometry.IPoint ptFrom = null;
			ESRI.ArcGIS.Geometry.IPoint ptTo = null;
			ESRI.ArcGIS.Geometry.IPoint pt0 = null;
			ESRI.ArcGIS.Geometry.IPoint Pt1 = null;
			ESRI.ArcGIS.Geometry.IPoint PtX = null;

			pClone = ((ESRI.ArcGIS.esriSystem.IClone)(pCutPolygon));
			pPolygon = ((ESRI.ArcGIS.Geometry.IPolygon2)(pClone.Clone()));

			pTopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pPolygon));
			pTopoOper.IsKnownSimple_2 = false;
			pTopoOper.Simplify();

			clipByPolyReturn = ((ESRI.ArcGIS.Geometry.IPolyline)(new ESRI.ArcGIS.Geometry.Polyline()));

			pCutLine = pClipLine;

			pGeomCol = ((ESRI.ArcGIS.Geometry.IGeometryCollection)(pPolygon));
			iRings = pGeomCol.GeometryCount;
			//     iRings = pPolygon.ExteriorRingCount

			if (iRings == 0)
			{
				//         ReDim pExteriorRing(iRings - 1)
				//     Else
				return clipByPolyReturn;
			}

			//     pPolygon.QueryExteriorRingsEx iRings, pExteriorRing(0)

			ptFrom = pCutLine.FromPoint;
			ptTo = pCutLine.ToPoint;

			dXl = ptTo.X - ptFrom.X;
			dYl = ptTo.Y - ptFrom.Y;

			pProxy = ((ESRI.ArcGIS.Geometry.IProximityOperator)(ptFrom));
			Cnt = 0;

			for (I = 0; I <= iRings - 1; I++)
			{
				// ' Getting each ring with its Index
				//         Set pRing = pExteriorRing(I)
				pRing = ((ESRI.ArcGIS.Geometry.IRing)(pGeomCol.get_Geometry(I)));
				pRingVertices = ((ESRI.ArcGIS.Geometry.IPointCollection)(pRing));

				N = pRingVertices.PointCount - 1;
				if (N >= 0)
				{
					System.Array.Resize<TIntersections>(ref pIntersectionPoints, N + Cnt + 1);

					L = -1;
					do
					{
						L = L + 1;
						Side = WhichSide(pCutLine, pRingVertices.get_Point(L));
					}
					while (Side == 0);

					// ' GetIntersectPointOfTwoLines
					for (J = 0; J <= N - 1; J++)
					{
						K = (L + 1) % N;
						Pt1 = pRingVertices.get_Point(K);

						Side1 = WhichSide(pCutLine, Pt1);
						if (Side1 * Side < 0)
						{
							pt0 = pRingVertices.get_Point(L);

							Side = Side1;
							dXs = Pt1.X - pt0.X;
							dYs = Pt1.Y - pt0.Y;
							Down = dYl * dXs - dXl * dYs;

							if (System.Math.Abs(Down) > Epsilon)
							{
								dX = pt0.X - ptFrom.X;
								dY = pt0.Y - ptFrom.Y;

								UpA = dXl * dY - dYl * dX;
								UpB = dXs * dY - dYs * dX;

								Down = 1.0 / Down;
								mA = UpA * Down;
								mB = UpB * Down;

								if ((mA >= 0.0 && mA < 1.0) && (mB >= 0.0 && mB < 1.0))
								{
									PtX = new ESRI.ArcGIS.Geometry.Point();
									PtX.PutCoords(pt0.X + mA * dXs, pt0.Y + mA * dYs);

									pIntersectionPoints[Cnt].pPoint = PtX;
									pIntersectionPoints[Cnt].Dist = pProxy.ReturnDistance(PtX);
									Cnt = Cnt + 1;
								}
							}
						}
						L = K;
					}
				}
			}

			pProxy = ((ESRI.ArcGIS.Geometry.IProximityOperator)(pPolygon));

			if (pProxy.ReturnDistance(ptTo) == 0)
			{
				PtX = new ESRI.ArcGIS.Geometry.Point();
				PtX.PutCoords(ptTo.X, ptTo.Y);
				pIntersectionPoints[Cnt].pPoint = PtX;
				pIntersectionPoints[Cnt].Dist = 0.0;
				Cnt = Cnt + 1;
			}

			if (pProxy.ReturnDistance(ptFrom) == 0)
			{
				PtX = new ESRI.ArcGIS.Geometry.Point();
				PtX.PutCoords(ptFrom.X, ptFrom.Y);
				pIntersectionPoints[Cnt].pPoint = PtX;
				pIntersectionPoints[Cnt].Dist = pCutLine.Length;
				Cnt = Cnt + 1;
			}

			UBd = Cnt - 1;

			if (UBd > 0)
			{
				pGeomCol = ((ESRI.ArcGIS.Geometry.IGeometryCollection)(clipByPolyReturn));

				System.Array.Resize<TIntersections>(ref pIntersectionPoints, UBd + 1);

				SortByDist(pIntersectionPoints, UBd);
				for (I = 0; I <= UBd; I += 2)
				{
					pPline = ((ESRI.ArcGIS.Geometry.IPath)(new ESRI.ArcGIS.Geometry.Path()));
					pPline.FromPoint = pIntersectionPoints[I].pPoint;
					pPline.ToPoint = pIntersectionPoints[I + 1].pPoint;
					pGeomCol.AddGeometry(pPline);
				}
			}
			return clipByPolyReturn;
		}

		private static void ArrangeVertices(TIntersections[] pInterSecPoints, ESRI.ArcGIS.Geometry.IPointCollection pVertexCol, TPointsHeap[] pPointsHeap)
		{
			int CurItem = 0;
			int I = 0;
			int J = 0;
			int K = 0;
			int L = 0;
			int N = 0;
			int M = 0;

			N = pInterSecPoints.GetUpperBound(0) + 1;
			M = pVertexCol.PointCount - 1;

			SortByDist(pInterSecPoints, N - 1);

			for (I = 0; I <= N - 1; I++)
				pInterSecPoints[I].IntersectIndex = I;

			SortByIndex(pInterSecPoints, N - 1);
			CurItem = 0;

			for (I = 0; I <= N - 1; I++)
			{
				J = (I + 1) % N;

				pPointsHeap[CurItem].pPoint = pInterSecPoints[I].pPoint;
				pPointsHeap[CurItem].IsIntersection = true;
				pPointsHeap[CurItem].IntersectIndex = pInterSecPoints[I].IntersectIndex;
				pInterSecPoints[I].PointIndex = CurItem;
				CurItem = CurItem + 1;

				K = (pInterSecPoints[I].PredIndex + 1) % M;
				L = (pInterSecPoints[J].PredIndex + 1) % M;
				while (K != L)
				{
					pPointsHeap[CurItem].pPoint = pVertexCol.get_Point(K);
					pPointsHeap[CurItem].IsIntersection = false;
					CurItem = CurItem + 1;
					K = (K + 1) % M;
				}
			}

			SortByDist(pInterSecPoints, N - 1);
		}

		private static void FormPolygons(TIntersections[] pIntersectionPoints, TPointsHeap[] pPointsHeap, ESRI.ArcGIS.Geometry.IPolyline pCutLine, ESRI.ArcGIS.Geometry.IPolygon pLeft, ESRI.ArcGIS.Geometry.IPolygon pRight, ESRI.ArcGIS.Geometry.IPolygon pUnspecified)
		{
			int StartIndex = 0;
			int EndIndex = 0;
			int I = 0;
			int J = 0;
			int K = 0;
			int N = 0;
			int M = 0;

			ESRI.ArcGIS.Geometry.IPointCollection pPoints = null;
			ESRI.ArcGIS.Geometry.IRing pRing = null;

			ESRI.ArcGIS.Geometry.IGeometryCollection pIUnspec = null;
			ESRI.ArcGIS.Geometry.IGeometryCollection pIRight = null;
			ESRI.ArcGIS.Geometry.IGeometryCollection pILeft = null;

			bool bUnspec = false;
			bool Trigger = false;

			pILeft = ((ESRI.ArcGIS.Geometry.IGeometryCollection)(pLeft));
			pIRight = ((ESRI.ArcGIS.Geometry.IGeometryCollection)(pRight));
			pIUnspec = ((ESRI.ArcGIS.Geometry.IGeometryCollection)(pUnspecified));

			N = pPointsHeap.GetUpperBound(0);
			M = pIntersectionPoints.GetUpperBound(0);

			for (J = 0; J <= M - 1; J += 2)
			{
				K = J + 1;

				StartIndex = pIntersectionPoints[J].PointIndex;
				EndIndex = pIntersectionPoints[K].PointIndex;

				I = StartIndex;
				K = (I + 1) % (N + 1);

				bUnspec = false;
				Trigger = false;

				pRing = ((ESRI.ArcGIS.Geometry.IRing)(new ESRI.ArcGIS.Geometry.Ring()));
				pPoints = ((ESRI.ArcGIS.Geometry.IPointCollection)(pRing));

				if ((pPointsHeap[I].UsedFlag & 1) == 0)
				{
					while (I != EndIndex)
					{
						if (pPointsHeap[I].IsIntersection)
						{
							pPoints.AddPoint(pPointsHeap[I].pPoint);

							if (Trigger)
							{
								K = I - 1;
								if (K < 0)
									K = K + N + 1;
								else
									K = K % (N + 1);

								if (WhichSide(pCutLine, pPointsHeap[K].pPoint) > 0)
								{
									pPointsHeap[I].UsedFlag = pPointsHeap[I].UsedFlag | 1;
									K = pPointsHeap[I].IntersectIndex - 1;
									if (K < 0)
										K = K + M + 1;
								}
								else
								{
									pPointsHeap[I].UsedFlag = pPointsHeap[I].UsedFlag | 2;
									K = pPointsHeap[I].IntersectIndex + 1;
									bUnspec = true;
								}

								K = K % (M + 1);
								I = (pIntersectionPoints[K].PointIndex);
							}
							else
							{
								pPointsHeap[I].UsedFlag = pPointsHeap[I].UsedFlag | 1;
								I = (I + 1) % (N + 1);
							}

							Trigger = !(Trigger);
						}
						else
						{
							pPoints.AddPoint(pPointsHeap[I].pPoint);
							I = (I + 1) % (N + 1);
						}
					}

					pPoints.AddPoint(pPointsHeap[I].pPoint);
					pRing.Close();

					if (bUnspec)
						pIUnspec.AddGeometry(pRing);
					else
						pILeft.AddGeometry(pRing);
				}

				pRing = ((ESRI.ArcGIS.Geometry.IRing)(new ESRI.ArcGIS.Geometry.Ring()));
				pPoints = ((ESRI.ArcGIS.Geometry.IPointCollection)(pRing));

				I = StartIndex;
				bUnspec = false;
				Trigger = false;

				K = I - 1;
				if (K < 0)
					K = K + N + 1;
				else
					K = K % (N + 1);

				if ((pPointsHeap[I].UsedFlag & 2) == 0)
				{
					while (I != EndIndex)
					{
						if (pPointsHeap[I].IsIntersection)
						{
							pPoints.AddPoint(pPointsHeap[I].pPoint);

							if (Trigger)
							{
								K = (I + 1) % (N + 1);

								if (WhichSide(pCutLine, pPointsHeap[K].pPoint) < 0)
								{
									pPointsHeap[I].UsedFlag = pPointsHeap[I].UsedFlag | 2;
									K = pPointsHeap[I].IntersectIndex - 1;
									if (K < 0)
										K = K + M + 1;
								}
								else
								{
									K = pPointsHeap[I].IntersectIndex + 1;
									pPointsHeap[I].UsedFlag = pPointsHeap[I].UsedFlag | 1;
									bUnspec = true;
								}

								K = K % (M + 1);
								I = (pIntersectionPoints[K].PointIndex);
							}
							else
							{
								pPointsHeap[I].UsedFlag = pPointsHeap[I].UsedFlag | 2;
								I = I - 1;
								if (I < 0)
									I = I + N + 1;
								I = I % (N + 1);
							}

							Trigger = !(Trigger);
						}
						else
						{
							pPoints.AddPoint(pPointsHeap[I].pPoint);

							I = I - 1;
							if (I < 0)
								I = I + N + 1;
							else
								I = I % (N + 1);
						}
					}

					pPoints.AddPoint(pPointsHeap[I].pPoint);
					pRing.Close();
					pRing.ReverseOrientation();

					if (bUnspec)
						pIUnspec.AddGeometry(pRing);
					else
						pIRight.AddGeometry(pRing);
				}
			}
		}

		private static int WhichSide(ESRI.ArcGIS.Geometry.IPolyline CutLine, ESRI.ArcGIS.Geometry.IPoint ptCheck)
		{
			int whichSideReturn = 0;
			double dX0 = 0;
			double dY0 = 0;
			double dX1 = 0;
			double dY1 = 0;
			double X = 0;

			dX0 = ptCheck.X - CutLine.FromPoint.X;
			dY0 = ptCheck.Y - CutLine.FromPoint.Y;

			dX1 = ptCheck.X - CutLine.ToPoint.X;
			dY1 = ptCheck.Y - CutLine.ToPoint.Y;

			X = dX0 * dY1 - dY0 * dX1;
			//     If Abs(X) > Epsilon Then
			whichSideReturn = System.Math.Sign(X);
			//     Else
			//         WhichSide = 0
			//     End If
			return whichSideReturn;
		}

		private static void QSortByDist(TIntersections[] A, int iLo, int iHi)
		{
			TIntersections T = new TIntersections();
			int Lo = iLo;
			int Hi = iHi;
			double Mid_Renamed = A[(Lo + Hi) >> 1].Dist;

			do
			{
				while (A[Lo].Dist < Mid_Renamed)
					Lo = Lo + 1;

				while (A[Hi].Dist > Mid_Renamed)
					Hi = Hi - 1;

				if (Lo <= Hi)
				{
					T = A[Lo];
					A[Lo] = A[Hi];
					A[Hi] = T;
					Lo = Lo + 1;
					Hi = Hi - 1;
				}
			}
			while (Lo <= Hi);

			if ((Hi > iLo))
				QSortByDist(A, iLo, Hi);

			if ((Lo < iHi))
				QSortByDist(A, Lo, iHi);
		}

		private static void SortByDist(TIntersections[] A, int N)
		{
			int Lo = A.GetLowerBound(0);
			int Hi = N;

			if (Lo == Hi)
				return;

			QSortByDist(A, Lo, Hi);
		}

		private static void QSortByIndex(TIntersections[] A, int iLo, int iHi)
		{
			TIntersections T = new TIntersections();
			int Lo = iLo;
			int Hi = iHi;
			int Mid_Renamed = A[(Lo + Hi) >> 1].PredIndex;

			do
			{
				while (A[Lo].PredIndex < Mid_Renamed)
					Lo = Lo + 1;

				while (A[Hi].PredIndex > Mid_Renamed)
					Hi = Hi - 1;

				if (Lo <= Hi)
				{
					T = A[Lo];
					A[Lo] = A[Hi];
					A[Hi] = T;
					Lo = Lo + 1;
					Hi = Hi - 1;
				}
			}
			while (Lo <= Hi);

			if ((Hi > iLo))
				QSortByIndex(A, iLo, Hi);

			if ((Lo < iHi))
				QSortByIndex(A, Lo, iHi);
		}

		private static void SortByIndex(TIntersections[] A, int N)
		{
			int Lo = A.GetLowerBound(0);
			int Hi = N;

			if (Lo == Hi)
				return;

			QSortByIndex(A, Lo, Hi);
		}
	}
}
