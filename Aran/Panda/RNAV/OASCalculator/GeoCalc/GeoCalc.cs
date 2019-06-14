using System;

namespace GeoCalc
{
	public static class GeoCalc
	{

		//const double Eps = 5e-16;

		const double Eps = 0.5e-12;
		const double rad = 180.0 / Math.PI;

		public static double FWG = 1.0 / 298.257223563;												//					= 0.0033528106647474805
		public static double E2WG = (2.0 - 1.0 / 298.257223563) / 298.257223563;					//E2WG = f*(2.0-f)  = 0.0066943799901413165;
		public static double AWG = 6378137.0;

		//#define	FWG		f
		//#define	E2WG	e2									//e2 = f*(2.0-f);
		//#define	AWG		a

		public static bool iterations = false;
		public static bool breakFlg = false;

		/*
		double AngularDistance(TGCoordinate Pnt0, TGCoordinate Pnt1)
		{
			double		TanU1, TanU2, CosU1, CosU2,
							SinU1, SinU2, SinL, CosL, fTmp;

			TanU1 = (1.0-FWG)*Math.Tan(DegToRad(Pnt0.Y));
			TanU2 = (1.0-FWG)*Math.Tan(DegToRad(Pnt1.Y));

			fTmp = Math.Atan(TanU1);
			CosU1 = Math.Cos(fTmp);
			SinU1 = Math.Sin(fTmp);

			fTmp = Math.Atan(TanU2);
			CosU2 = Math.Cos(fTmp);
			SinU2 = Math.Sin(fTmp);

			fTmp = DegToRad(Pnt1.X-Pnt0.X);
			SinL = Math.Sin(fTmp);
			CosL = Math.Cos(fTmp);

			return 1.0-SinU1*SinU2-CosU1*CosU2*CosL;
		}

		double NearDistance(TGCoordinate Pnt0, TGCoordinate Pnt1)
		{
			double		TanU1, TanU2, CosU1, CosU2,
							SinU1, SinU2, SinL, CosL, fTmp,
							Rm, fX0, fX1, fY0, fY1;

			fX0 = DegToRad(Pnt0.X);
			fX1 = DegToRad(Pnt1.X);
			fY0 = DegToRad(Pnt0.Y);
			fY1 = DegToRad(Pnt1.Y);

			TanU1 = (1.0-FWG)*Math.Tan(fY0);
			TanU2 = (1.0-FWG)*Math.Tan(fY1);

			fTmp = Math.Atan(TanU1);
			CosU1 = Math.Cos(fTmp);
			SinU1 = Math.Sin(fTmp);

			fTmp = Math.Atan(TanU2);
			CosU2 = Math.Cos(fTmp);
			SinU2 = Math.Sin(fTmp);

			fTmp = Math.Sin(0.5*(fY0+fY1));
			Rm = AWG*(1.0-E2WG)/(1.0-E2WG*fTmp*fTmp);

			fTmp = fX1-fX0;
			SinL = Math.Sin(fTmp);
			CosL = Math.Cos(fTmp);

			fTmp = SinU1*SinU2+CosU1*CosU2*CosL;
			fTmp = fTmp > 1.0 ? 1.0 : fTmp;
			fTmp = fTmp < -1.0 ? -1.0 : fTmp;
			return Math.Acos(fTmp)*Rm;
		}
		*/

		public static TGCoordinate DirectProblem(TGCoordinate BPnt, double Az, double D, out double InAz)
		{
			double _A, B, C, U1, CosU1, SinU1,
			TanU1, C1, U2, SinA, sinCi, cosCi,
			dC, Ci, Ci0, dCi, C2m, CosC2m, fTmp,
			cosAz, sinAz, Sgn_Chg_cur, CosL,
			Sgn_Chg_prev, fX0, fX1, fY0, fY1,
			SinY1, CosY1, L;

			TGCoordinate Result;

			Sgn_Chg_cur = double.MaxValue;

			Az = Utils.DegToRad(Az);
			fX0 = Utils.DegToRad(Utils.Mod360(BPnt.X));
			fY0 = Utils.DegToRad(BPnt.Y);

			TanU1 = (1.0 - FWG) * Math.Tan(fY0);

			U1 = Math.Atan(TanU1);
			SinU1 = Math.Sin(U1);
			CosU1 = Math.Cos(U1);

			//	if(Math.Abs(SinU1)-1.0 == 0.0)
			//	{
			//		CosU1 = (Math.Sign(SinU1)*0.5*Math.PI - fY0)*(1.0-FWG);
			//		SinU1 = Math.Sign(SinU1)*(1.0-CosU1)*(1.0+CosU1);
			//		TanU1 = SinU1/CosU1;
			//	}
			/*	else
					CosU1 = Math.Cos(U1);
			*/
			cosAz = Math.Cos(Az);
			sinAz = Math.Sin(Az);

			C1 = 2.0 * Math.Atan2(TanU1, cosAz);
			SinA = CosU1 * sinAz;

			U2 = (1.0 + SinA) * (1.0 - SinA) * E2WG / (1.0 - E2WG);
			_A = 1.0 + U2 * (4096.0 + U2 * (-768.0 + U2 * (320.0 - 175.0 * U2))) / 16384.0;
			B = U2 * (256.0 + U2 * (-128.0 + U2 * (74.0 - 47.0 * U2))) / 1024.0;

			dCi = D / (AWG * Math.Sqrt(1.0 - E2WG) * _A);
			Ci0 = dCi;

			do
			{
				Sgn_Chg_prev = Sgn_Chg_cur;
				Ci = Ci0;
				C2m = C1 + Ci;
				CosC2m = Math.Cos(C2m);
				dC = B * Math.Sin(Ci) * (CosC2m + B * (Math.Cos(Ci) * (-1.0 + 2.0 * Utils.Sqr(CosC2m)) -
					 B * CosC2m * (-3.0 + 4.0 * Utils.Sqr(Math.Sin(Ci))) * (-3.0 + 4.0 * Utils.Sqr(CosC2m)) / 6.0) * 0.25);

				Ci0 = dCi + dC;

				System.Windows.Forms.Application.DoEvents();
				if (breakFlg)
					break;

				Sgn_Chg_cur = Math.Abs(Ci0 - Ci);
			}
			while (Sgn_Chg_cur < Sgn_Chg_prev);

			Ci = Ci0;

			CosC2m = Math.Cos(C1 + Ci);
			sinCi = Math.Sin(Ci);
			cosCi = Math.Cos(Ci);

			fTmp = (SinU1 * cosCi + CosU1 * sinCi * cosAz);

			if (Math.Abs(fTmp) < Eps)
				fY1 = 0.0;
			else
				fY1 = Math.Atan2(fTmp, (1.0 - FWG) * Math.Sqrt(Utils.Sqr(SinA) +
							Utils.Sqr(SinU1 * sinCi - CosU1 * cosCi * cosAz)));

			Result.Y = Utils.RadToDeg(fY1);
			//------------------------------------------
			if (Math.Abs(CosU1) >= Eps)
			{
				fTmp = sinCi * sinAz;
				dCi = CosU1 * cosCi - SinU1 * sinCi * cosAz;

				if (Math.Abs(fTmp) < Eps && Math.Abs(dCi) < Eps)
					dCi = (1.0 + cosCi) < Eps ? Math.PI : 0.0;
				else
					dCi = Math.Atan2(fTmp, dCi);
			}
			else if (SinU1 > 0.0)
				dCi = Math.PI - Az;
			else
				dCi = Az;
			//------------------------------------------
			fTmp = (1.0 - SinA) * (1.0 + SinA);
			C = FWG * fTmp * (4.0 + (4.0 * FWG - 3.0 * FWG * fTmp)) / 16.0;
			fX1 = Utils.Mod2PI(fX0 + dCi - (1.0 - C) * FWG * SinA * (Ci + C * sinCi * (CosC2m + C * cosCi * (-1.0 + 2.0 * Utils.Sqr(CosC2m)))));


			Result.X = Utils.RadToDeg(fX1);

			CosY1 = Math.Cos(fY1);
			SinY1 = Math.Sin(fY1);

			L = fX1 - fX0;

			if (CosY1 != 0.0)
			{
				CosL = Math.Cos(L);
				if (Math.Abs(SinA) < Eps)
					InAz = 0.0;
				else
					InAz = Math.Atan2(SinA, CosU1 * cosCi * cosAz - SinU1 * sinCi);

				if (InAz > 0.0)
				{
					if ((Math.Abs(fX1 - fX0) < Math.PI && fX0 < fX1) || (Math.Abs(fX1 - fX0) > Math.PI && fX0 > fX1))
						InAz += Math.PI;
				}
				else if (InAz < 0.0)
				{
					if ((Math.Abs(fX1 - fX0) < Math.PI && fX0 > fX1) || (Math.Abs(fX1 - fX0) > Math.PI && fX0 < fX1))
						InAz += Math.PI;
				}
				else if (CosL < 0.0)
					InAz = (fY1 + fY0 < 0.0) ? Math.PI : 0.0;
				else
					InAz = (fY0 < fY1) ? Math.PI : 0.0;
			}
			else if (SinY1 > 0.0)
				InAz = Math.PI + L;
			else
				InAz = -L;

			InAz = Utils.Mod360(Utils.RadToDeg(InAz));
			return Result;
		}

		static void InversForNears(TGCoordinate Coordinate0, TGCoordinate Coordinate1, out double Dist, out double Az0, out double Az1, out double Sigm)
		{
			double
			SinL, fTmp, L, M,
			EWG, EBG,
			SinAz0, CosAz0,
			SinAz1, CosAz1,
			SinY0, CosY0,
			SinY1, CosY1,
			N0, R0, N1, R1,
			fX0, fX1, fY0, fY1;

			fX0 = Utils.DegToRad(Utils.Mod360(Coordinate0.X));
			fY0 = Utils.DegToRad(90.0 - Coordinate0.Y);
			fX1 = Utils.DegToRad(Utils.Mod360(Coordinate1.X));
			fY1 = Utils.DegToRad(90.0 - Coordinate1.Y);

			SinY0 = Math.Cos(fY0);
			CosY0 = Math.Sin(fY0);

			SinY1 = Math.Cos(fY1);
			CosY1 = Math.Sin(fY1);

			L = fX1 - fX0;
			if (Math.Abs(L) > Math.PI)
				L = -Math.Sign(L) * (Math.PI + Math.PI - Math.Abs(L));

			SinL = Math.Sin(L);
			M = fY0 - fY1;
			//==========================================
			EWG = Math.Sqrt(E2WG);

			EBG = AWG * (1.0 - EWG) * (1.0 + EWG);

			N0 = AWG / Math.Sqrt((1.0 - EWG * SinY0) * (1.0 + EWG * SinY0));
			N1 = AWG / Math.Sqrt((1.0 - EWG * SinY1) * (1.0 + EWG * SinY1));

			fTmp = (1.0 - EWG * SinY0) * (1.0 + EWG * SinY0);
			R0 = EBG / (fTmp * Math.Sqrt(fTmp));

			fTmp = (1.0 - EWG * SinY1) * (1.0 + EWG * SinY1);
			R1 = EBG / (fTmp * Math.Sqrt(fTmp));

			CosAz0 = Math.Sqrt(R0 * R1) * M;
			SinAz0 = Math.Sqrt(N1 * CosY1 * N0 * CosY0) * L;

			CosAz1 = -CosAz0;
			SinAz1 = -SinAz0;

			Dist = Math.Sqrt(CosAz0 * CosAz0 + SinAz0 * SinAz0);
			if (Dist < Eps)
			{
				Az0 = 0.0;
				Az1 = 0.0;
				Sigm = 0.0;
				return;
			}

			L = fX1 - fX0;

			if (Math.Abs(CosY0) > Eps)
			{
				if (Math.Abs(M) < Eps)
					Az0 = (SinL > 0.0 ? 0.0 : Math.PI) + 0.5 * Math.PI;
				else
					Az0 = Math.Atan2(SinAz0, CosAz0);
			}
			else if (SinY0 > 0.0)
				Az0 = Math.PI - L;
			else
				Az0 = L;

			if (Math.Abs(CosY1) > Eps)
			{
				if (Math.Abs(M) < Eps)
					Az1 = (SinL < 0.0 ? 0.0 : Math.PI) + 0.5 * Math.PI;
				else
					Az1 = Math.Atan2(SinAz1, CosAz1);
			}
			else if (SinY1 > 0.0)
				Az1 = Math.PI + L;
			else
				Az1 = -L;

			Sigm = Dist / Math.Sqrt(R0 * N0);

			Az0 = Utils.Mod360(Utils.RadToDeg(Az0));
			Az1 = Utils.Mod360(Utils.RadToDeg(Az1));
		}

		static double CalcSinAlpha(double L, double U1, double U2, out double Sigm)
		{
			double SinL, CosL, SinU1, CosU1, SinU2, CosU2,
						SinSigm, CosSigm, Cos2Sigm, SinA,
						Cos2A, fTmp, C, L0 = L, dL;

			SinU1 = Math.Sin(U1);
			CosU1 = Math.Cos(U1);

			SinU2 = Math.Sin(U2);
			CosU2 = Math.Cos(U2);

			do
			{
				SinL = Math.Sin(L);
				CosL = Math.Cos(L);

				CosSigm = SinU1 * SinU2 + CosU1 * CosU2 * CosL;
				SinSigm = Utils.Hypot(CosU2 * SinL, CosU1 * SinU2 - SinU1 * CosU2 * CosL);

				Sigm = Math.Atan2(SinSigm, CosSigm);
				fTmp = CosU1 * CosU2 * SinL;

				if (Math.Abs(SinSigm) > Eps)
					SinA = CosU1 * CosU2 * SinL / SinSigm;
				else
					SinA = 0.0;

				Cos2A = (1.0 - SinA) * (1.0 + SinA);

				if (Cos2A > Eps)
					Cos2Sigm = CosSigm - 2.0 * SinU1 * SinU2 / Cos2A;
				else
					Cos2Sigm = CosSigm;

				C = FWG * Cos2A * (4.0 + FWG * (4.0 - 3.0 * Cos2A)) / 16.0;

				fTmp = L;
				dL = (1.0 - C) * FWG * SinA * (Sigm + C * SinSigm * (Cos2Sigm + C * CosSigm * (2.0 * Utils.Sqr(Cos2Sigm) - 1.0)));
				L = L0 + dL;

				System.Windows.Forms.Application.DoEvents();
				if (breakFlg) break;
			}
			while (Math.Abs(fTmp - L) > Eps);
			return SinA;
		}

		public static void InversByVincenty(TGCoordinate Coordinate0, TGCoordinate Coordinate1, out double Dist, out double Az0, out double Az1, out double Sigm)
		{
			double signL0, L0,
			CosU1, CosU2, SinU1, SinU2,
			CosLon, SinLon, TanU1, TanU2,
			SinL, CosL, SinSigm, CosSigm,
			dSigm, Cos2Sgm, SinA, Cos2A,
			A, C, B, fTmp, L, U1, U2, u2,
			fX0, fX1, fY0, fY1;

			//char[] Buff = new char[128];
			//ChartForm->Reset();
			//sprintf(Buff, "x1=%010.8Lf y1=%010.8Lf", Coordinate0.X, Coordinate0.Y);
			//ChartForm->ListBox1->Items->Add(Buff);
			//sprintf(Buff, "x2=%010.8Lf y2=%010.8Lf", Coordinate1.X, Coordinate1.Y);
			//ChartForm->ListBox1->Items->Add(Buff);

			fX0 = Utils.DegToRad(Utils.Mod360(Coordinate0.X));
			fY0 = Utils.DegToRad(Coordinate0.Y);
			fX1 = Utils.DegToRad(Utils.Mod360(Coordinate1.X));
			fY1 = Utils.DegToRad(Coordinate1.Y);

			fTmp = Math.Tan(fY0);
			TanU1 = fTmp - fTmp * FWG;

			U1 = Math.Atan(TanU1);
			CosU1 = Math.Cos(U1);
			SinU1 = Math.Sin(U1);

			fTmp = Math.Tan(fY1);
			TanU2 = fTmp - fTmp * FWG;

			U2 = Math.Atan(TanU2);
			SinU2 = Math.Sin(U2);
			CosU2 = Math.Cos(U2);

			L0 = fX1 - fX0;

			signL0 = Math.Sign(L0);
			//MainForm->Edit1->Text = "";
			//MainForm->Edit2->Text = "";

			if (Math.Abs(L0) > Math.PI)
			{
				L0 = -signL0 * (Math.PI + Math.PI - Math.Abs(L0));
				signL0 = -signL0;
			}

			L = L0;
			u2 = 0.0;

			do
			{
				SinL = Math.Sin(L);
				CosL = Math.Cos(L);

				CosSigm = SinU1 * SinU2 + CosU1 * CosU2 * CosL;
				SinSigm = Utils.Hypot(CosU2 * SinL, CosU1 * SinU2 - SinU1 * CosU2 * CosL);

				Sigm = Math.Atan2(SinSigm, CosSigm);
				fTmp = CosU1 * CosU2 * SinL;

				if (Math.Abs(SinSigm) > Eps)
					SinA = CosU1 * CosU2 * SinL / SinSigm;
				else
					SinA = 0.0;

				Cos2A = (1.0 - SinA) * (1.0 + SinA);

				if (Cos2A > Eps)
					Cos2Sgm = CosSigm - 2.0 * SinU1 * SinU2 / Cos2A;
				else
					Cos2Sgm = CosSigm;

				C = FWG * Cos2A * (4.0 + FWG * (4.0 - 3.0 * Cos2A)) / 16.0;

				u2 = L;
				L = (1.0 - C) * FWG * SinA * (Sigm + C * SinSigm * (Cos2Sgm + C * CosSigm * (2.0 * Utils.Sqr(Cos2Sgm) - 1.0)));
				L += L0;

				//if (SinA != 0.0)			ChartForm->AddValue(SinA, utils.RadToDeg(u2 / SinA));
				//else						ChartForm->AddValue(SinA, 100000.0);

				System.Windows.Forms.Application.DoEvents();
				if (breakFlg)
					break;
			}
			while (Math.Abs(u2 - L) > Eps);

			u2 = (Cos2A) * E2WG / (1.0 - E2WG);
			A = u2 * (4096.0 + u2 * (-768.0 + u2 * (320.0 - 175.0 * u2))) / 16384.0;
			B = u2 * (256.0 + u2 * (-128.0 + u2 * (74.0 - 47.0 * u2))) / 1024.0;

			dSigm = B * SinSigm * (Cos2Sgm + B * (CosSigm * (-1.0 + 2.0 * Utils.Sqr(Cos2Sgm)) -
					B * Cos2Sgm * (-3.0 + 4.0 * Utils.Sqr(SinSigm)) * (-3.0 + 4.0 * Utils.Sqr(Cos2Sgm)) / 6.0) / 4.0);
			//===========================================================
			Dist = AWG * (1.0 - FWG) * (Sigm - dSigm);
			Dist = Dist + Dist * A;

			L0 = fX1 - fX0;

			CosLon = Math.Cos(L0);
			SinLon = Math.Sin(L0);
			//------------------------- Az0 -----------------------------
			if (Math.Abs(CosU1) >= Eps)
			{
				if (Math.Abs(SinLon) < Eps) Az0 = 0.0;
				else
					Az0 = Math.Atan2(CosU2 * SinL, (CosU1 * SinU2 - SinU1 * CosU2 * CosL));

				if (Az0 > 0.0)
				{
					if (Math.Abs(L0) < Math.PI && fX1 < fX0) Az0 += Math.PI;
					if (Math.Abs(L0) > Math.PI && fX1 > fX0) Az0 += Math.PI;
				}
				else if (Az0 < 0.0)
				{
					if (Math.Abs(L0) < Math.PI && fX1 > fX0) Az0 += Math.PI;
					if (Math.Abs(L0) > Math.PI && fX1 < fX0) Az0 += Math.PI;
				}
				else if (CosLon < 0.0) Az0 = fY0 + fY1 < 0.0 ? Math.PI : 0.0;
				else Az0 = fY1 < fY0 ? Math.PI : 0.0;
			}
			else if (SinU1 > 0.0) Az0 = Math.PI - L0;
			else Az0 = L0;
			//========================= Az1 =============================
			if (Math.Abs(CosU2) >= Eps)
			{
				if (Math.Abs(SinLon) < Eps) Az1 = 0.0;
				else
					Az1 = Math.Atan2(CosU1 * SinL, CosU1 * SinU2 * CosL - SinU1 * CosU2);

				if (Az1 > 0.0)
				{
					if (Math.Abs(L0) < Math.PI && fX0 < fX1) Az1 += Math.PI;
					if (Math.Abs(L0) > Math.PI && fX0 > fX1) Az1 += Math.PI;
				}
				else if (Az1 < 0.0)
				{
					if (Math.Abs(L0) < Math.PI && fX0 > fX1) Az1 += Math.PI;
					if (Math.Abs(L0) > Math.PI && fX0 < fX1) Az1 += Math.PI;
				}
				else if (CosLon < 0.0) Az1 = fY0 + fY1 < 0.0 ? Math.PI : 0.0;
				else Az1 = fY0 < fY1 ? Math.PI : 0.0;
			}
			else if (SinU2 > 0.0) Az1 = Math.PI + L0;
			else Az1 = -L0;
			//------------------------------------------------------
			Az0 = Utils.Mod360(Utils.RadToDeg(Az0));
			Az1 = Utils.Mod360(Utils.RadToDeg(Az1));
		}

		public static bool InverseProblem(TGCoordinate Coordinate0, TGCoordinate Coordinate1, out double Dist, out double Az0, out double Az1, out double Sigm, out bool antipod)
		{
			double signL0, L0,
			CosU1, CosU2, SinU1, SinU2,
			TanU1, TanU2, SinL, CosL, fTmp,
			SinSigm, CosSigm, dSigm, dL, dlMax,
			dU, rMax, Cos2Sgm, SinA, Cos2A,
			A, C, B, L, U1, U2, u2, fX0, fX1,
			fY0, fY1, A1, B1, Z1, sAng, sA,
			cA, D, z, c, y2, y3, C90, D0, D1,
			dSigm0, dL0, dL1, LAnti,
			dy, dy0, dy1, dX, dX0, dX1, SinA0, SinA1;


			TGCoordinate AntiCoord;
			//char[] Buff = new char[128];

			//ChartForm->Reset();

			//sprintf(Buff, "x1=%010.8Lf y1=%010.8Lf", Coordinate0.X, Coordinate0.Y);
			//ChartForm->ListBox1->Items->Add(Buff);

			//sprintf(Buff, "x2=%010.8Lf y2=%010.8Lf", Coordinate1.X, Coordinate1.Y);
			//ChartForm->ListBox1->Items->Add(Buff);

			InversForNears(Coordinate0, Coordinate1, out Dist, out Az0, out Az1, out Sigm);

			antipod = false;
			if (Dist <= 200.0)
				return false;

			fX0 = Utils.DegToRad(Utils.Mod360(Coordinate0.X));
			fY0 = Utils.DegToRad(Coordinate0.Y);
			fX1 = Utils.DegToRad(Utils.Mod360(Coordinate1.X));
			fY1 = Utils.DegToRad(Coordinate1.Y);

			fTmp = Math.Tan(fY0);
			TanU1 = fTmp - fTmp * FWG;
			U1 = Math.Atan(TanU1);

			fTmp = Math.Tan(fY1);
			TanU2 = fTmp - fTmp * FWG;
			U2 = Math.Atan(TanU2);

			CosU1 = Math.Cos(U1);
			SinU1 = Math.Sin(U1);

			SinU2 = Math.Sin(U2);
			CosU2 = Math.Cos(U2);

			L0 = fX1 - fX0;
			signL0 = Math.Sign(Math.Sin(L0));

			dL = Math.PI - L0;
			if (dL > Math.PI)
				dL -= 2.0 * Math.PI;

			dU = Math.Abs(fY0 + fY1);

			Cos2A = Utils.Sqr(SinU1);
			C90 = FWG * Cos2A * (4.0 + FWG * (4.0 - 3.0 * Cos2A)) / 16.0;
			dlMax = Math.Sign(dL) * Math.PI * (1.0 - C90) * FWG * CosU1;

			AntiCoord.Y = -Coordinate0.Y;
			AntiCoord.X = Utils.Mod360(Coordinate0.X + 180.0);

			if (Math.Abs(dL) < Math.Abs(dlMax) && dU < Math.Abs(dlMax + dlMax))
			{
				A1 = Math.Sqrt(1.0 - E2WG * Utils.Sqr(CosU1)) * Math.Abs(U1 + U2) / CosU1;
				B1 = Math.PI * FWG * CosU1;
				Z1 = 0.005;

				do
				{
					z = Z1;
					y2 = 1.0 - Z1 * Z1;
					y3 = y2 * Math.Sqrt(y2);

					c = B1 * (1.0 - FWG * y2 * (4.0 + FWG * (4.0 - 3.0 * y2)) / 16.0);

					Z1 = Z1 - (c * Z1 * y3 + A1 * Z1 * y2 - dL * y3) / (c * y3 + A1 * y2 + A1 * Z1 * Z1);

					System.Windows.Forms.Application.DoEvents();
					if (breakFlg) break;

					if (Z1 < -1.0 || Z1 > 1.0)
					{
						Z1 = Math.Sign(Z1) * (1.0 - Eps);
						break;
					}
				} while (Math.Abs(Z1 - z) > Eps);

				y2 = 1.0 - Z1 * Z1;

				SinA = Z1 * CosU1;
				Cos2A = (1.0 - SinA) * (1.0 + SinA);

				C = FWG * Cos2A * (4.0 + FWG * (4.0 - 3.0 * Cos2A)) / 16.0;
				D = (1.0 - C) * FWG * Math.PI;
				//ChartForm->AddValue(SinA, D);

				AntiCoord.X -= Utils.RadToDeg(D * SinA);
				InversForNears(Coordinate1, AntiCoord, out Dist, out Az0, out Az1, out Sigm);
				if (Z1 == 1.0)
					Sigm = Math.PI - Math.Sqrt(Utils.Sqr(U1 + U2) + Utils.Sqr(CosU1 * D * SinA));
				else
					Sigm = Math.PI - Math.Abs(U1 + U2) / Math.Sqrt(y2);

				CosSigm = Math.Cos(Sigm);
				SinSigm = Math.Sin(Sigm);
			}
			else
			{
				InversForNears(Coordinate1, AntiCoord, out Dist, out  Az0, out Az1, out Sigm);
				Sigm = Math.PI - Sigm;
				CosSigm = Math.Cos(Sigm);
				SinSigm = Math.Sin(Sigm);

				SinA = CosU1 * Math.Sin(Utils.DegToRad(Az0));
				Cos2A = (1.0 - SinA) * (1.0 + SinA);

				C = FWG * Cos2A * (4.0 + FWG * (4.0 - 3.0 * Cos2A)) / 16.0;
				D = (1.0 - C) * FWG * Math.PI;
				//ChartForm->AddValue(SinA, D);
				Dist = 1000.0;
			};

			if (Cos2A > Eps)
				Cos2Sgm = CosSigm - 2.0 * SinU1 * SinU2 / Cos2A;
			else
				Cos2Sgm = CosSigm;

			fTmp = (1.0 - C) * FWG * (Sigm + C * SinSigm * (Cos2Sgm + C * CosSigm * (2.0 * Utils.Sqr(Cos2Sgm) - 1.0)));

			rMax = fTmp * SinA;
			fTmp *= CosU1;

			//MainForm->Edit1->Text = "";
			//MainForm->Edit2->Text = "";

			if (Math.Abs(dL) > Math.Abs(3.5 * dlMax) || dU > Math.Abs(3.5 * dlMax))			//	not antipodal	case
			{
				L = L0 + rMax;
				do
				{
					SinL = Math.Sin(L);
					CosL = Math.Cos(L);

					CosSigm = SinU1 * SinU2 + CosU1 * CosU2 * CosL;
					SinSigm = Utils.Hypot(CosU2 * SinL, CosU1 * SinU2 - SinU1 * CosU2 * CosL);

					Sigm = Math.Atan2(SinSigm, CosSigm);
					fTmp = CosU1 * CosU2 * SinL;

					if (Math.Abs(SinSigm) > Eps)
						SinA = CosU1 * CosU2 * SinL / SinSigm;
					else
						SinA = 0.0;

					Cos2A = (1.0 - SinA) * (1.0 + SinA);

					if (Cos2A > Eps)
						Cos2Sgm = CosSigm - 2.0 * SinU1 * SinU2 / Cos2A;
					else
						Cos2Sgm = CosSigm;

					C = FWG * Cos2A * (4.0 + FWG * (4.0 - 3.0 * Cos2A)) / 16.0;

					fTmp = L;
					L = (1.0 - C) * FWG * SinA * (Sigm + C * SinSigm * (Cos2Sgm + C * CosSigm * (2.0 * Utils.Sqr(Cos2Sgm) - 1.0)));

					//if (SinA != 0.0)					ChartForm->AddValue(SinA, L / SinA);
					//else								ChartForm->AddValue(SinA, 100000.0);

					L += L0;

					System.Windows.Forms.Application.DoEvents();
					if (breakFlg) break;
				}
				while (Math.Abs(fTmp - L) > Eps);
			}
			else if (dU <= 100000.0 * Eps && Math.Abs(dL) <= Math.Abs(dlMax))
			{
				do
				{
					fTmp = SinA;
					C = FWG * Cos2A * (4.0 + FWG * (4.0 - 3.0 * Cos2A)) / 16.0;
					D = (1.0 - C) * FWG * Math.PI;

					SinA = dL / D;
					//ChartForm->AddValue(SinA, D);
					Cos2A = (1.0 - SinA) * (1.0 + SinA);
				} while (Math.Abs(fTmp - SinA) >= Eps);

				Sigm = Math.PI;
				SinSigm = 0.0;
				CosSigm = -1.0;

				if (Cos2A > Eps)
					Cos2Sgm = CosSigm - 2.0 * SinU1 * SinU2 / Cos2A;
				else
					Cos2Sgm = CosSigm;

				SinL = 0.0;
				CosL = -1.0;
			}
			else					//	Antipodal	case
			{
				LAnti = Utils.Mod360(Coordinate0.X + 180.0) - Coordinate1.X;
				if (LAnti > 180.0)
					LAnti = LAnti - 360.0;

				L0 = Utils.DegToRad(LAnti);
				dlMax *= Math.Sign(L0);

				if (Math.Abs(L0) < Math.Abs(dlMax))
					dlMax = L0;

				//=============================================
				SinA1 = SinA;

				dy1 = D * SinA;
				dL1 = L0 - dy1;
				SinA1 = CalcSinAlpha(dL1, -U1, U2, out dSigm);

				Cos2A = (1.0 - SinA1) * (1.0 + SinA1);
				C = FWG * Cos2A * (4.0 + FWG * (4.0 - 3.0 * Cos2A)) / 16.0;
				dX1 = (1.0 - C) * FWG * SinA1 * Math.PI;
				D1 = (dy1 - dX1) * dSigm;
				//=============================================

				dy0 = 0.5 * dy1;
				dL0 = L0 - dy0;
				SinA0 = CalcSinAlpha(dL0, -U1, U2, out dSigm0);

				Cos2A = (1.0 - SinA0) * (1.0 + SinA0);
				C = FWG * Cos2A * (4.0 + FWG * (4.0 - 3.0 * Cos2A)) / 16.0;
				dX0 = (1.0 - C) * FWG * SinA0 * Math.PI;
				D0 = (dy0 - dX0) * dSigm0;
				//=============================================

				//ChartForm->AddValue(SinA0, dSigm0);
				//ChartForm->AddValue(SinA1, dSigm);

				dSigm = dSigm0;

				do
				{
					if (Math.Abs(D1 - D0) < 1e-5 * Eps * dSigm)
						break;

					dy = dy1 - (dy1 - dy0) * D1 / (D1 - D0);
					dL = L0 - dy;

					SinA = CalcSinAlpha(dL, -U1, U2, out dSigm);

					Cos2A = (1.0 - SinA) * (1.0 + SinA);
					C = FWG * Cos2A * (4.0 + FWG * (4.0 - 3.0 * Cos2A)) / 16.0;
					dX = (1.0 - C) * FWG * SinA * Math.PI;
					D = (dy - dX) * dSigm;

					//ChartForm->AddValue(SinA, dSigm);

					D0 = D1;
					D1 = D;

					dy0 = dy1;
					dy1 = dy;

					System.Windows.Forms.Application.DoEvents();
					if (breakFlg)
						break;

				} while (Math.Abs(D) > Eps * dSigm);

				Sigm = Math.PI - dSigm;
				SinSigm = Math.Sin(Sigm);
				CosSigm = Math.Cos(Sigm);

				if (Cos2A > Eps)
					Cos2Sgm = CosSigm - 2.0 * SinU1 * SinU2 / Cos2A;
				else
					Cos2Sgm = CosSigm;

				SinL = SinA * SinSigm / (CosU1 * CosU2);
				CosL = -Math.Sqrt((1.0 - SinL) * (1.0 + SinL));
			}

			//fTmp = utils.Mod360((utils.RadToDeg((fX0 + Math.PI * (1.0 - (1.0 - C) * FWG * SinA)))));
			//MainForm->Edit1->Text = (double)fTmp;

			//fTmp = 1.0 - CosU1 * CosU1;
			//fTmp = FWG * fTmp * (4.0 + FWG * (4.0 - 3.0 * fTmp)) / 16.0;
			//MainForm->Edit2->Text = utils.Mod360((utils.RadToDeg((fX0 + Math.PI * (1.0 - signL0 * (1.0 - fTmp) * FWG * CosU1)))));

			u2 = (Cos2A) * E2WG / (1.0 - E2WG);
			A = u2 * (4096.0 + u2 * (-768.0 + u2 * (320.0 - 175.0 * u2))) / 16384.0;
			B = u2 * (256.0 + u2 * (-128.0 + u2 * (74.0 - 47.0 * u2))) / 1024.0;

			dSigm = B * SinSigm * (Cos2Sgm + B * (CosSigm * (-1.0 + 2.0 * Utils.Sqr(Cos2Sgm)) -
					B * Cos2Sgm * (-3.0 + 4.0 * Utils.Sqr(SinSigm)) * (-3.0 + 4.0 * Utils.Sqr(Cos2Sgm)) / 6.0) / 4.0);
			//===========================================================
			Dist = AWG * (1.0 - FWG) * (Sigm - dSigm);
			Dist = Dist + Dist * A;

			L0 = fX1 - fX0;

			sAng = CosU1 * SinU2 - SinU1 * CosU2 * CosL;

			if (CosU1 > Eps)
			{
				sA = SinA / CosU1;
				if (sA > 1.0) sA = 1.0;
				else if (sA < -1.0) sA = -1.0;

				cA = Math.Sqrt((1.0 - sA) * (1.0 + sA));
				cA *= Math.Sign(sAng);
				Az0 = Math.Atan2(sA, cA);
			}
			else
				Az0 = Math.PI;

			sA = -SinA * SinSigm;
			cA = -SinU2 * CosSigm + SinU1;
			if ((Math.Abs(sA) < Eps && Math.Abs(cA) < Eps) || SinSigm < Eps)
				Az1 = -Az0;
			else
				Az1 = Math.Atan2(sA, cA);
			//------------------------------------------------------/
			Az0 = Utils.Mod360(Utils.RadToDeg(Az0));
			Az1 = Utils.Mod360(Utils.RadToDeg(Az1));

			return true;
		}

		/*
		public static bool InverseProblem1(TGCoordinate Coordinate0, TGCoordinate Coordinate1, out double Dist, out double Az0, out double Az1, out double Sigm, out bool antipod)
		{
			double signL0, L0,
			SinY0, SinY1, CosU1, CosU2,
			SinU1, SinU2, CosLon, SinLon,
			TanU1, TanU2, SinL, CosL, fTmp,
			Sgn_Chg_prev, Sgn_Chg_cur,
			SinSigm, CosSigm, dSigm,
			Cos2Sgm, SinA, Cos2A, A, C, B,
			L, U1, U2, u2, fX0, fX1, fY0, fY1;

			//char	Buff[128];

			//ChartForm->Reset();
			//sprintf(Buff,"x1=%010.8Lf y1=%010.8Lf", Coordinate0.X, Coordinate0.Y);
			//ChartForm->ListBox1->Items->Add(Buff);
			//sprintf(Buff,"x2=%010.8Lf y2=%010.8Lf", Coordinate1.X, Coordinate1.Y);
			//ChartForm->ListBox1->Items->Add(Buff);

			InversForNears(Coordinate0, Coordinate1, out Dist, out Az0, out Az1, out Sigm);

			antipod = false;
			if (Dist <= 200.0)
				return false;

			fX0 = Utils.DegToRad(Utils.Mod360(Coordinate0.X));
			fY0 = Utils.DegToRad(Coordinate0.Y);
			fX1 = Utils.DegToRad(Utils.Mod360(Coordinate1.X));
			fY1 = Utils.DegToRad(Coordinate1.Y);

			Sgn_Chg_cur = double.MaxValue;

			//	TanU1 = (1.0-FWG)*Math.Tan(fY0);
			fTmp = Math.Tan(fY0);
			TanU1 = fTmp - fTmp * FWG;

			U1 = Math.Atan(TanU1);
			CosU1 = Math.Cos(U1);
			SinU1 = Math.Sin(U1);

			//	TanU2 = (1.0-FWG)*Math.Tan(fY1);
			fTmp = Math.Tan(fY1);
			TanU2 = fTmp - fTmp * FWG;

			U2 = Math.Atan(TanU2);
			SinU2 = Math.Sin(U2);
			CosU2 = Math.Cos(U2);

			L0 = fX1 - fX0;
			signL0 = Math.Sign(L0);
			//MainForm->Edit2->Text = "";
			//MainForm->Edit1->Text = "";

			double A1, B1, C1, Z1;

			if (Math.Abs(L0) > Math.PI)
			{
				L0 = -signL0 * (Math.PI + Math.PI - Math.Abs(L0));
				signL0 = -signL0;
			}
			//	else

			L = L0;

			//Polchoose = Math.Sign(Math.Sin(L));
			u2 = 0.0;

			do
			{
				Sgn_Chg_prev = Sgn_Chg_cur;
				SinL = Math.Sin(L);
				CosL = Math.Cos(L);

				CosSigm = SinU1 * SinU2 + CosU1 * CosU2 * CosL;
				SinSigm = Utils.Hypot(CosU2 * SinL, CosU1 * SinU2 - SinU1 * CosU2 * CosL);

				if (CosSigm > 1.0) Sigm = 0.0;
				else if (CosSigm < -1.0) Sigm = Math.PI;
				else Sigm = Math.Acos(CosSigm);

				fTmp = CosU1 * CosU2 * SinL;

				if (Sigm > Math.PI * 0.8)
				{
					C1 = Math.Abs(Math.PI - Math.Abs(L0));
					A1 = Math.Sqrt(1.0 - E2WG * Utils.Sqr(CosU1)) * Math.Abs(U1 + U2) / CosU1;
					B1 = Math.PI * FWG * CosU1;
					Z1 = 0.0;
					double z, c, sina;
					do
					{
						z = Z1;
						sina = Utils.Sqr(Math.Cos(Math.Atan(Z1)));
						c = FWG * sina * (4.0 + FWG * (4.0 - 3.0 * sina)) / 16.0;
						Z1 = C1 / A1 - B1 * (1.0 - c) * Z1 / (A1 * (1.0 + Utils.Sqr(Z1)));
					} while (Math.Abs(Z1 - z) > Eps);
					SinA = Math.Sin(Math.Atan(Z1)) * CosU1;
					antipod = true;
				}
				else
				{
					if (Math.Abs(SinSigm) > Eps)
						SinA = CosU1 * CosU2 * SinL / SinSigm;
					else
						SinA = 0.0;
				}

				Cos2A = (1.0 - SinA) * (1.0 + SinA);

				if (Cos2A > Eps)
					Cos2Sgm = CosSigm - 2.0 * SinU1 * SinU2 / Cos2A;
				else
					Cos2Sgm = CosSigm;

				C = FWG * Cos2A * (4.0 + FWG * (4.0 - 3.0 * Cos2A)) / 16.0;

				L = (1.0 - C) * FWG * SinA * (Sigm + C * SinSigm * (Cos2Sgm + C * CosSigm * (2.0 * Utils.Sqr(Cos2Sgm) - 1.0)));
				//Sgn_Chg_cur = Math.Abs(Math.Abs(u2)-Math.Abs(L));

				if (antipod)
				{
					fTmp = double.MaxValue;
					double dL, D, Phi = U1 + U2;

					L = (Math.PI - Math.Abs(L0));		//		L = Math.PI;
					L *= Math.Sign(L * L0);

					//if (false)
					//{
					//	Cos2A = 0.5;
					//	SinA = Math.Sqrt(0.5);
					//}

					Sigm = Math.PI - Math.Abs(Phi);
					SinSigm = Math.Sin(Sigm);
					CosSigm = Math.Cos(Sigm);
					dL = 0.0;

					do
					{
						fTmp = SinA;
						C = FWG * Cos2A * (4.0 + FWG * (4.0 - 3.0 * Cos2A)) / 16.0;
						if (Cos2A > Eps)
							Cos2Sgm = CosSigm - 2.0 * SinU1 * SinU2 / Cos2A;
						else
							Cos2Sgm = CosSigm;

						D = (1.0 - C) * FWG * (Sigm + C * SinSigm * (Cos2Sgm + C * CosSigm * (2.0 * Utils.Sqr(Cos2Sgm) - 1.0)));

						u2 = L - dL;
						SinA = u2 / D;
						//ChartForm->AddValue(SinA, Utils.RadToDeg(D));

						Cos2A = (1.0 - SinA) * (1.0 + SinA);
						SinL = SinA * SinSigm / (CosU1 * CosU2);
						CosL = -Math.Sqrt(1.0 - SinL * SinL);
						dL = Math.Atan2(SinL, -CosL);
						SinSigm = Utils.Hypot(CosU2 * SinL, CosU1 * SinU2 - SinU1 * CosU2 * CosL);
						CosSigm = SinU1 * SinU2 + CosU1 * CosU2 * CosL;
						Sigm = Math.Atan2(SinSigm, CosSigm);
					} while (Math.Abs(fTmp - SinA) >= Eps);

					//fTmp = Utils.Mod360((Utils.RadToDeg((fX0 + Math.PI * (1.0 - (1.0 - C) * FWG * SinA)))));
					//MainForm->Edit1->Text = (double)fTmp;

					//fTmp = 1.0 - CosU1 * CosU1;
					//fTmp = FWG * fTmp * (4.0 + FWG * (4.0 - 3.0 * fTmp)) / 16.0;

					//MainForm->Edit2->Text = (double)Utils.Mod360((Utils.RadToDeg((fX0 + Math.PI * (1.0 - signL0 * (1.0 - fTmp) * FWG * CosU1)))));
					break;
				}
				u2 = L;
				L += L0;
				//if (SinA != 0.0)					ChartForm->AddValue(SinA, Utils.RadToDeg(u2 / SinA));

				System.Windows.Forms.Application.DoEvents();
				if (breakFlg)
					break;
			}
			while (Sgn_Chg_cur > Eps);

			u2 = (Cos2A) * E2WG / (1.0 - E2WG);
			A = u2 * (4096.0 + u2 * (-768.0 + u2 * (320.0 - 175.0 * u2))) / 16384.0;
			B = u2 * (256.0 + u2 * (-128.0 + u2 * (74.0 - 47.0 * u2))) / 1024.0;

			dSigm = B * SinSigm * (Cos2Sgm + B * (CosSigm * (-1.0 + 2.0 * Utils.Sqr(Cos2Sgm)) -
					B * Cos2Sgm * (-3.0 + 4.0 * Utils.Sqr(SinSigm)) * (-3.0 + 4.0 * Utils.Sqr(Cos2Sgm)) / 6.0) / 4.0);
			//===========================================================
			Dist = AWG * (1.0 - FWG) * (Sigm - dSigm);
			Dist = Dist + Dist * A;

			L0 = fX1 - fX0;

			CosLon = Math.Cos(L0);
			SinLon = Math.Sin(L0);
			SinY0 = Math.Sin(fY0);
			SinY1 = Math.Sin(fY1);
			//------------------------- Az0 -----------------------------
			if (Math.Abs(CosU1) >= Eps)
			{
				if (Math.Abs(SinLon) < Eps) Az0 = 0.0;
				else if (antipod)
				{
					fTmp = SinA / Math.Cos(fY0) * Math.Sqrt(1.0 - E2WG * SinY0 * SinY0);
					//if(fTmp <= -1.0)					Az0 = -0.5*Math.PI;
					//else
					if (fTmp >= 1.0)					Az0 = 0.5 * Math.PI;
					else								Az0 = Math.Sign(fY0 + fY1) * Math.Asin(fTmp);
				}
				else Az0 = Math.Atan2(CosU2 * SinL,
						 (CosU1 * SinU2 - SinU1 * CosU2 * CosL));

				if (Az0 > 0.0)
				{
					if (Math.Abs(L0) < Math.PI && fX1 < fX0) Az0 += Math.PI;
					if (Math.Abs(L0) > Math.PI && fX1 > fX0) Az0 += Math.PI;
				}
				else if (Az0 < 0.0)
				{
					if (Math.Abs(L0) < Math.PI && fX1 > fX0) Az0 += Math.PI;
					if (Math.Abs(L0) > Math.PI && fX1 < fX0) Az0 += Math.PI;
				}
				else if (CosLon < 0.0) Az0 = fY0 + fY1 < 0.0 ? Math.PI : 0.0;
				else Az0 = fY1 < fY0 ? Math.PI : 0.0;
			}
			else if (SinU1 > 0.0) Az0 = Math.PI - L0;
			else Az0 = L0;
			//========================= Az1 =============================
			if (Math.Abs(CosU2) >= Eps)
			{
				if (Math.Abs(SinLon) < Eps)
					Az1 = 0.0;
				else if (antipod)
				{
					fTmp = SinA / Math.Cos(fY1) * Math.Sqrt(1.0 - E2WG * SinY1 * SinY1);
					//if(fTmp <= -1.0)				Az1 = -0.5*Math.PI;
					//else
					if (fTmp >= 1.0)				Az1 = 0.5 * Math.PI;
					else							Az1 = -Math.Sign(fY0 + fY1) * Math.Asin(fTmp);
				}
				else								Az1 = Math.Atan2(CosU1 * SinL, (CosU1 * SinU2 * CosL - SinU1 * CosU2));

				if (Az1 > 0.0)
				{
					if (Math.Abs(L0) < Math.PI && fX0 < fX1) Az1 += Math.PI;
					if (Math.Abs(L0) > Math.PI && fX0 > fX1) Az1 += Math.PI;
				}
				else if (Az1 < 0.0)
				{
					if (Math.Abs(L0) < Math.PI && fX0 > fX1) Az1 += Math.PI;
					if (Math.Abs(L0) > Math.PI && fX0 < fX1) Az1 += Math.PI;
				}
				else if (CosLon < 0.0)
					Az1 = fY0 + fY1 < 0.0 ? Math.PI : 0.0;
				else
					Az1 = fY0 < fY1 ? Math.PI : 0.0;
			}
			else if (SinU2 > 0.0)
				Az1 = Math.PI + L0;
			else
				Az1 = -L0;
			//------------------------------------------------------
			Az0 = Utils.Mod360(Utils.RadToDeg(Az0));
			Az1 = Utils.Mod360(Utils.RadToDeg(Az1));

			return true;
		}

		public static bool InverseProblem2(TGCoordinate Coordinate0, TGCoordinate Coordinate1, out double Dist, out double Az0, out double Az1, out double Sigm, out bool antipod)
		{
			long i;//, Polchoose;
			double signL0, L0,
			SinY0, SinY1, CosU1, CosU2,
			SinU1, SinU2, CosLon, SinLon,
			TanU1, TanU2, SinL, CosL, fTmp,
			Sgn_Chg_prev, Sgn_Chg_cur,
			SinSigm, CosSigm, dSigm,
			Cos2Sgm, SinA, Cos2A, A, C, B,
			L, U1, U2, u2, fX0, fX1, fY0, fY1;

			antipod = false;

			InversForNears(Coordinate0, Coordinate1, out Dist, out Az0, out Az1, out Sigm);

			if (Dist <= 200.0)
				return false;

			fX0 = Utils.DegToRad(Utils.Mod360(Coordinate0.X));
			fY0 = Utils.DegToRad(Coordinate0.Y);
			fX1 = Utils.DegToRad(Utils.Mod360(Coordinate1.X));
			fY1 = Utils.DegToRad(Coordinate1.Y);

			Sgn_Chg_cur = double.MaxValue;

			//	TanU1 = (1.0-FWG)*Math.Tan(fY0);
			fTmp = Math.Tan(fY0);
			TanU1 = fTmp - fTmp * FWG;

			U1 = Math.Atan(TanU1);
			CosU1 = Math.Cos(U1);
			SinU1 = Math.Sin(U1);

			//	TanU2 = (1.0-FWG)*Math.Tan(fY1);
			fTmp = Math.Tan(fY1);
			TanU2 = fTmp - fTmp * FWG;

			U2 = Math.Atan(TanU2);
			SinU2 = Math.Sin(U2);
			CosU2 = Math.Cos(U2);

			L0 = fX1 - fX0;
			signL0 = Math.Sign(L0);

			//Form1->Edit2->Text = "";
			//Form1->Edit1->Text = "";

			if (Math.Abs(L0) > Math.PI)
			{
				L0 = -signL0 * (Math.PI + Math.PI - Math.Abs(L0));
				signL0 = -signL0;
			}
			//	else
			L = L0;

			//	Polchoose = Math.Sign(Math.Sin(L));
			u2 = 0.0;

			do
			{
				Sgn_Chg_prev = Sgn_Chg_cur;
				SinL = Math.Sin(L);
				CosL = Math.Cos(L);

				CosSigm = SinU1 * SinU2 + CosU1 * CosU2 * CosL;
				SinSigm = Utils.Hypot(CosU2 * SinL, CosU1 * SinU2 - SinU1 * CosU2 * CosL);

				if (CosSigm > 1.0) Sigm = 0.0;
				else if (CosSigm < -1.0) Sigm = Math.PI;
				else Sigm = Math.Acos(CosSigm);

				fTmp = CosU1 * CosU2 * SinL;

				if (Math.Abs(SinSigm) > Eps)
					SinA = CosU1 * CosU2 * SinL / SinSigm;
				else
					SinA = 0.0;

				Cos2A = (1.0 - SinA) * (1.0 + SinA);

				if (Cos2A > Eps)
					Cos2Sgm = CosSigm - 2.0 * SinU1 * SinU2 / Cos2A;
				else
					Cos2Sgm = CosSigm;

				C = FWG * Cos2A * (4.0 + FWG * (4.0 - 3.0 * Cos2A)) / 16.0;

				L = (1.0 - C) * FWG * SinA * (Sigm + C * SinSigm * (Cos2Sgm + C * CosSigm * (2.0 * Utils.Sqr(Cos2Sgm) - 1.0)));
				Sgn_Chg_cur = Math.Abs(Math.Abs(u2) - Math.Abs(L));

				if ((Sgn_Chg_cur > Sgn_Chg_prev) || (Sgn_Chg_cur == Sgn_Chg_prev && u2 * L <= 0.0))
				{
					antipod = true;
					fTmp = double.MaxValue;
					double oldL = L;
					long j;

					L = signL0 * Math.PI;	//		L = Math.PI;
					SinL = Math.Sin(L);
					CosL = Math.Cos(L);

					for (j = 0; j < 1600; j++)
					{
						CosSigm = SinU1 * SinU2 + CosU1 * CosU2 * CosL;
						fTmp = CosSigm * CosSigm;

						if (fTmp > 1.0) { CosSigm = Math.Sign(CosSigm); SinSigm = 0.0; }
						else SinSigm = Math.Sqrt((1.0 - CosSigm) * (1.0 + CosSigm));

						if (CosSigm > 1.0) Sigm = 0.0;
						else if (CosSigm < -1.0) Sigm = Math.PI;
						else Sigm = Math.Acos(CosSigm);

						for (i = 0; i < 130; i++)
						{
							fTmp = SinA;
							SinA = (L - L0) / ((1.0 - C) * FWG * (Sigm + C * SinSigm *
												(Cos2Sgm + C * CosSigm * (2.0 * Utils.Sqr(Cos2Sgm) - 1.0))));
							Cos2A = (1.0 - SinA) * (1.0 + SinA);

							if (Cos2A > Eps)
								Cos2Sgm = CosSigm - 2.0 * SinU1 * SinU2 / Cos2A;
							else
								Cos2Sgm = CosSigm;

							C = FWG * Cos2A * (4.0 + FWG * (4.0 - 3.0 * Cos2A)) / 16.0;

							if (Math.Abs(SinA - fTmp) < Eps)
								break;
						}

						A = Math.Asin(SinA);
						SinL = SinA * SinSigm / (CosU1 * CosU2);
						if (SinL > 1.0) { L = 0.5 * Math.PI; SinL = 1.0; }
						else if (SinL < -1.0) { L = Math.PI + 0.5 * Math.PI; SinL = -1.0; }
						else L = signL0 * Math.PI - Math.Asin(SinL);

						CosL = Math.Cos(L);
						if (Math.Abs(oldL - L) < Eps)
							break;
						oldL = L;
					}

					//fTmp = Utils.Mod360((Utils.RadToDeg((fX0 + Math.PI * (1.0 - (1.0 - C) * FWG * SinA)))));
					//Form1->Edit1->Text = (double)fTmp;

					//fTmp = j;
					//fTmp = 1.0 - CosU1*CosU1;
					//fTmp = FWG*fTmp*(4.0+FWG*(4.0-3.0*fTmp))/16.0;
					//Form1->Edit2->Text = (double)Utils.Mod360((Utils.RadToDeg((fX0 + Math.PI * (1.0 - signL0 * (1.0 - fTmp) * FWG * CosU1)))));
					break;
				}
				u2 = L;
				L += L0;
				System.Windows.Forms.Application.DoEvents();
				if (breakFlg) break;
			}
			while (Sgn_Chg_cur > Eps);

			u2 = (Cos2A) * E2WG / (1.0 - E2WG);
			A = u2 * (4096.0 + u2 * (-768.0 + u2 * (320.0 - 175.0 * u2))) / 16384.0;
			B = u2 * (256.0 + u2 * (-128.0 + u2 * (74.0 - 47.0 * u2))) / 1024.0;

			dSigm = B * SinSigm * (Cos2Sgm + B * (CosSigm * (-1.0 + 2.0 * Utils.Sqr(Cos2Sgm)) -
					B * Cos2Sgm * (-3.0 + 4.0 * Utils.Sqr(SinSigm)) * (-3.0 + 4.0 * Utils.Sqr(Cos2Sgm)) / 6.0) / 4.0);
			//===========================================================
			Dist = AWG * (1.0 - FWG) * (Sigm - dSigm);
			Dist = Dist + Dist * A;

			L0 = fX1 - fX0;

			CosLon = Math.Cos(L0);
			SinLon = Math.Sin(L0);
			SinY0 = Math.Sin(fY0);
			SinY1 = Math.Sin(fY1);
			//------------------------- Az0 -----------------------------
			if (Math.Abs(CosU1) >= Eps)
			{
				if (Math.Abs(SinLon) < Eps) Az0 = 0.0;
				else if (antipod)
				{
					fTmp = SinA / Math.Cos(fY0) * Math.Sqrt(1.0 - E2WG * SinY0 * SinY0);
					//			if(fTmp <= -1.0)					Az0 = -0.5*Math.PI;
					//			else
					if (fTmp >= 1.0) Az0 = 0.5 * Math.PI;
					else Az0 = Math.Sign(fY0 + fY1) * Math.Asin(fTmp);
				}
				else Az0 = Math.Atan2(CosU2 * SinL,
						 (CosU1 * SinU2 - SinU1 * CosU2 * CosL));

				if (Az0 > 0.0)
				{
					if (Math.Abs(L0) < Math.PI && fX1 < fX0) Az0 += Math.PI;
					if (Math.Abs(L0) > Math.PI && fX1 > fX0) Az0 += Math.PI;
				}
				else if (Az0 < 0.0)
				{
					if (Math.Abs(L0) < Math.PI && fX1 > fX0) Az0 += Math.PI;
					if (Math.Abs(L0) > Math.PI && fX1 < fX0) Az0 += Math.PI;
				}
				else if (CosLon < 0.0) Az0 = fY0 + fY1 < 0.0 ? Math.PI : 0.0;
				else Az0 = fY1 < fY0 ? Math.PI : 0.0;
			}
			else if (SinU1 > 0.0) Az0 = Math.PI - L0;
			else Az0 = L0;
			//========================= Az1 =============================
			if (Math.Abs(CosU2) >= Eps)
			{
				if (Math.Abs(SinLon) < Eps) Az1 = 0.0;
				else if (antipod)
				{
					fTmp = SinA / Math.Cos(fY1) * Math.Sqrt(1.0 - E2WG * SinY1 * SinY1);
					//if(fTmp <= -1.0)					Az1 = -0.5*Math.PI;
					//else
					if (fTmp >= 1.0)
						Az1 = 0.5 * Math.PI;
					else
						Az1 = -Math.Sign(fY0 + fY1) * Math.Asin(fTmp);
				}
				else
					Az1 = Math.Atan2(CosU1 * SinL, (CosU1 * SinU2 * CosL - SinU1 * CosU2));

				if (Az1 > 0.0)
				{
					if (Math.Abs(L0) < Math.PI && fX0 < fX1) Az1 += Math.PI;
					if (Math.Abs(L0) > Math.PI && fX0 > fX1) Az1 += Math.PI;
				}
				else if (Az1 < 0.0)
				{
					if (Math.Abs(L0) < Math.PI && fX0 > fX1) Az1 += Math.PI;
					if (Math.Abs(L0) > Math.PI && fX0 < fX1) Az1 += Math.PI;
				}
				else if (CosLon < 0.0) Az1 = fY0 + fY1 < 0.0 ? Math.PI : 0.0;
				else Az1 = fY0 < fY1 ? Math.PI : 0.0;
			}
			else if (SinU2 > 0.0) Az1 = Math.PI + L0;
			else Az1 = -L0;
			//------------------------------------------------------
			Az0 = Utils.Mod360(Utils.RadToDeg(Az0));
			Az1 = Utils.Mod360(Utils.RadToDeg(Az1));

			return true;
		}
		*/

		public static double ReturnGeodesicAzimuth(string Pt0X, string Pt0Y, string Pt1X, string Pt1Y)
		{
			bool antipod;
			TGCoordinate Coordinate0, Coordinate1;
			double Dist, Az0, Az1, Sigm;

			Coordinate0.X = double.Parse(Pt0X);
			Coordinate0.Y = double.Parse(Pt0Y);
			Coordinate1.X = double.Parse(Pt1X);
			Coordinate1.Y = double.Parse(Pt1Y);

			InverseProblem(Coordinate0, Coordinate1, out Dist, out Az0, out Az1, out Sigm, out antipod);
			return Az0;
		}

		public static double ReturnGeodesicDistance(string Pt0X, string Pt0Y, string Pt1X, string Pt1Y, string Res)
		{
			bool antipod;
			TGCoordinate Coordinate0, Coordinate1;
			double Dist, Az0, Az1, Sigm;

			Coordinate0.X = double.Parse(Pt0X);
			Coordinate0.Y = double.Parse(Pt0Y);
			Coordinate1.X = double.Parse(Pt1X);
			Coordinate1.Y = double.Parse(Pt1Y);

			InverseProblem(Coordinate0, Coordinate1, out Dist, out Az0, out Az1, out Sigm, out antipod);

			return Dist;

		}

		public static void ReturnPointAlongGeodesic(string Pt0X, string Pt0Y, string Dist, string Azt, out double Pt1X, out double Pt1Y)
		{
			TGCoordinate Coordinate0, Coordinate1;
			double AIn0, az, d;

			Coordinate0.X = double.Parse(Pt0X);
			Coordinate0.Y = double.Parse(Pt0Y);
			az = double.Parse(Azt);
			d = double.Parse(Dist);

			Coordinate1 = DirectProblem(Coordinate0, az, d, out AIn0);
			Pt1X = Coordinate1.X;
			Pt1Y = Coordinate1.Y;
		}

		/*
		public static double ReturnAntipodalDistance(TGCoordinate Pt0, TGCoordinate Pt1, out double Az0, out double Az1)
		{
			bool antipod;

			double Dist, Sigm;
			double AppAz, fTmp;
			double dL, dB, dA0, dA1, ds, M, N;
			double SinAz1, CosAz1, CosY1, SinY1;


			TGCoordinate AppPt;

			Pt0.X = Utils.Mod360(Pt0.X);
			Pt1.X = Utils.Mod360(Pt1.X);

			InverseProblem(Pt0, Pt1, out Dist, out Az0, out Az1, out Sigm, out antipod);
			//	if(!antipod)
			//		return Dist;

			double EWG = Math.Sqrt(E2WG);
			double EBG = AWG * (1.0 - EWG) * (1.0 + EWG);

			do
			{
				AppPt = DirectProblem(Pt0, Az0, Dist, out AppAz);

				dL = Utils.DegToRad(Pt1.X - AppPt.X);
				dB = Utils.DegToRad(Pt1.Y - AppPt.Y);

				SinAz1 = Math.Sin(Utils.DegToRad(AppAz));
				CosAz1 = Math.Sin(Utils.DegToRad(AppAz));
				CosY1 = Math.Cos(Utils.DegToRad(AppPt.Y));

				SinY1 = Math.Sin(Utils.DegToRad(AppPt.Y));

				SinY1 = Math.Sign(SinY1) * Math.Sqrt((1.0 + CosY1) * (1.0 - CosY1));

				N = AWG / Math.Sqrt((1.0 - EWG * SinY1) * (1.0 + EWG * SinY1));

				fTmp = (1.0 - EWG * SinY1) * (1.0 + EWG * SinY1);
				M = EBG / (fTmp * Math.Sqrt((1.0 - EWG * SinY1) * (1.0 + EWG * SinY1)));

				ds = M * CosAz1 * dB - N * CosY1 * SinAz1 * dL;
				dA0 = (M * SinAz1 * dB - N * CosY1 * CosAz1 * dL) / Dist;
				dA1 = (M * SinAz1 * dB + N * CosY1 * CosAz1 * dL) / Dist;

				Az0 += Utils.RadToDeg(dA0);
				Az1 += Utils.RadToDeg(dA1);
				Dist += ds;
			}
			while (ds * ds > 2 * Eps);

			Az1 = AppAz;
			return Dist;
		}

		//Rassim1 + Firdowsy
		public static double ReturnAntipodalDistanceRasFi(TGCoordinate Pt0, TGCoordinate Pt1, out double Az0, out double Az1)
		{
			int i;
			bool antipod;

			double Sigm, AppSigm;

			double AppAz, Azm1, Az1m, dAz;
			double Dist, Dm, Dm0, ds;
			double Az00, Az1m0, v0, Dm0v0, Dm1v1;
			double DistBC, AzBC, AzCB;
			double Alpha, fTmp;

			TGCoordinate AppPt, AppPt0;

			Pt0.X = Utils.Mod360(Pt0.X);
			Pt1.X = Utils.Mod360(Pt1.X);

			InverseProblem(Pt0, Pt1, out Dist, out Az0, out Az1, out Sigm, out antipod);
			//	if(!antipod)
			//		return Dist;

			AppPt = DirectProblem(Pt0, Az0, Dist, out AppAz);

			InversForNears(Pt1, AppPt, out Dm, out Az1m, out Azm1, out AppSigm);

			if (Dm * Dm < Eps)
				return Dist;

			Az00 = Az0;
			Az1m0 = Az1m;
			Alpha = Utils.DegToRad(Az1m - AppAz);

			//	v0 = Math.Sin(Alpha);
			//	Dm0v0 = Dm*v0;
			//	Alpha = Math.Asin (v0);
			//	u1 = Az0 +	rad*Math.Sign(Math.Sin(DegToRad(Pt1.Y)))*
			//				Math.Abs(Alpha*Math.Sin(AppSigm)/
			//				Math.Sin(Math.PI*(1.0-FWG)));

			double DistIst, SinDistIst, CostDistIst;

			Dm0 = Dm;
			//	Dm0v0 = Dm*Math.Sin(Alpha);

			v0 = Math.Cos(Alpha);
			Dist += Dm * v0;

			CostDistIst = Math.Cos(Dist / AWG) * Math.Cos(Dm / AWG) +
									Math.Sin(Dist / AWG) * Math.Sin(Dm / AWG) * v0;

			CostDistIst = CostDistIst > 1.0 ? 1.0 : CostDistIst;
			CostDistIst = CostDistIst < -1.0 ? -1.0 : CostDistIst;

			DistIst = Math.Acos(CostDistIst);
			SinDistIst = Math.Sin(DistIst);

			fTmp = (Math.Cos(Dm / AWG) - Math.Cos(Dist / AWG) * CostDistIst) / (Math.Sin(Dist / AWG) * SinDistIst);
			//	fTmp = (Math.Cos(Dm/AWG)-Math.Cos(Dist/AWG)*CostDistIst)/(Math.Sin(Dist/AWG)*SinDistIst);
			//	fTmp = fTmp > 1.0 ? 1.0 : fTmp;
			//	fTmp = fTmp < -1.0 ? -1.0 : fTmp;

			if (fTmp > 1.0) Az0 -= 1e-5;
			else if (fTmp < -1.0) Az0 += 1e-5;
			else Az0 -= rad * Math.Sign(v0) * (Math.Acos(fTmp));

			i = 0;
			AppPt0 = AppPt;
			do
			{
				AppPt = DirectProblem(Pt0, Az0, Dist, out Az1);
				InversForNears(AppPt, AppPt0, out DistBC, out AzBC, out AzCB, out AppSigm);

				InversForNears(Pt1, AppPt, out Dm, out Az1m, out Azm1, out AppSigm);

				Alpha = Utils.DegToRad(Az1m - Az1);
				Dm1v1 = Dm * Math.Sin(Alpha);

				ds = Dm * Math.Cos(Alpha);
				Dist += ds;

				Alpha = Utils.DegToRad(Az1m0 - Az1);
				Dm0v0 = Dm0 * Math.Sin(Alpha);

				fTmp = Az0;
				Az0 = (Az0 - Az00 * Dm1v1 / Dm0v0) / (1.0 - Dm1v1 / Dm0v0);
				//		Az0 = (Az0*Dm0v0 - Az00*Dm1v1)/(Dm0v0-Dm1v1);
				Az00 = fTmp;
				dAz = Az0 - Az00;
				Dm0 = Dm;
				Az1m0 = Az1m;
				AppPt0 = AppPt;
				i++;
			}
			while (Math.Abs(100 * dAz * ds) > Eps);

			//char	Buff[128];
			//sprintf(Buff, "Iterations: %d", i);
			//MessageDlg(Buff, mtInformation, TMsgDlgButtons() << mbOK, 0);

			string msg = string.Format("Iterations: {0}", i);
			System.Windows.Forms.MessageBox.Show(msg, null, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

			return Dist;
		}

		//Rassim
		public static double ReturnAntipodalDistanceRassim(TGCoordinate Pt0, TGCoordinate Pt1, out double Az0, out double Az1)
		{
			int i;
			bool antipod;

			double Sigm, AppSigm;

			double AppAz, Azm1, Az1m, dAz;
			double Dist, Dm, Dm0, ds;
			double Az00, Az1m0, v0, Dm0v0, Dm1v1;
			double Alpha, fTmp;

			TGCoordinate AppPt;

			Pt0.X = Utils.Mod360(Pt0.X);
			Pt1.X = Utils.Mod360(Pt1.X);

			InverseProblem(Pt0, Pt1, out Dist, out Az0, out Az1, out Sigm, out antipod);
			//	if(!antipod)
			//		return Dist;

			AppPt = DirectProblem(Pt0, Az0, Dist, out AppAz);

			InversForNears(Pt1, AppPt, out Dm, out Az1m, out Azm1, out AppSigm);

			if (Dm * Dm < Eps)
				return Dist;

			Az00 = Az0;
			Az1m0 = Az1m;
			Alpha = Utils.DegToRad(Az1m - AppAz);

			//	v0 = Math.Sin(Alpha);
			//	Dm0v0 = Dm*v0;
			//	Alpha = Math.Asin (v0);
			//	u1 = Az0 +	rad*Math.Sign(Math.Sin(DegToRad(Pt1.Y)))*
			//				Math.Abs(Alpha*Math.Sin(AppSigm)/
			//				Math.Sin(Math.PI*(1.0-FWG)));

			double DistIst, SinDistIst, CostDistIst;

			Dm0 = Dm;
			Dm0v0 = Dm * Math.Sin(Alpha);

			v0 = Math.Cos(Alpha);
			Dist += Dm * v0;

			CostDistIst = Math.Cos(Dist / AWG) * Math.Cos(Dm / AWG) + Math.Sin(Dist / AWG) * Math.Sin(Dm / AWG) * v0;

			CostDistIst = CostDistIst > 1.0 ? 1.0 : CostDistIst;
			CostDistIst = CostDistIst < -1.0 ? -1.0 : CostDistIst;

			DistIst = Math.Acos(CostDistIst);
			SinDistIst = Math.Sin(DistIst);

			fTmp = (Math.Cos(Dm / AWG) - Math.Cos(Dist / AWG) * CostDistIst) / (Math.Sin(Dist / AWG) * SinDistIst);
			//	fTmp = (Math.Cos(Dm/AWG)-Math.Cos(Dist/AWG)*CostDistIst)/(Math.Sin(Dist/AWG)*SinDistIst);
			//	fTmp = fTmp > 1.0 ? 1.0 : fTmp;
			//	fTmp = fTmp < -1.0 ? -1.0 : fTmp;

			if (fTmp > 1.0) { }					//		Az0 -= 1e-5;
			else if (fTmp < -1.0) { }			//		Az0 += 1e-5;
			else Az0 -= rad * Math.Sign(v0) * (Math.Acos(fTmp));

			i = 0;
			do
			{
				AppPt = DirectProblem(Pt0, Az0, Dist, out Az1);
				InversForNears(Pt1, AppPt, out Dm, out Az1m, out Azm1, out AppSigm);

				Alpha = Utils.DegToRad(Az1m - Az1);
				Dm1v1 = Dm * Math.Sin(Alpha);

				ds = Dm * Math.Cos(Alpha);
				Dist += ds;

				dAz = (Az0 - Az00);
				Az00 = Az0;
				Az0 -= Dm1v1 * dAz / (Dm1v1 - Dm0v0);
				Dm0v0 = Dm1v1;

				i++;
			}
			while (Math.Abs(dAz * ds) > Eps);

			//char	Buff[128];
			//sprintf(Buff, "Iterations: %d", i);
			//MessageDlg(Buff, mtInformation,					TMsgDlgButtons() << mbOK, 0);

			string msg = string.Format("Iterations: {0}", i);
			System.Windows.Forms.MessageBox.Show(msg, null, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

			return Dist;
		}
		*/

		/*
			Betta = DegToRad(AppAz0-Az0m);
			v0 = Math.Sin(Betta);

			CostDistIst = Math.Cos(Dist/AWG)*Math.Cos(Dm0/AWG) + Math.Sin(Dist/AWG)*Math.Sin(Dm0/AWG)*Math.Cos(Betta);

			CostDistIst = CostDistIst > 1.0 ? 1.0 : CostDistIst;
			CostDistIst = CostDistIst < -1.0 ? -1.0 : CostDistIst;
			DistIst = Math.Acos(CostDistIst);
			SinDistIst = Math.Sin(DistIst);

		//	u1 = u0 + rad*Math.Sign(Math.Sin(y1))*Math.Asin (Math.Abs(v0)*Math.Sin(AppSigm)/Math.Sin(Math.PI*(1.0-FWG)));

			fTmp = (Math.Cos(Dm0/AWG)-Math.Cos(Dist/AWG)*CostDistIst)/(Math.Sin(Dist/AWG)*SinDistIst);
		//	fTmp = fTmp > 1.0 ? 0.999999999999 : fTmp;
		//	fTmp = fTmp < -1.0 ? -0.9999999999 : fTmp;
			fTmp = fTmp > 1.0 ? 1.0 : fTmp;
			fTmp = fTmp < -1.0 ? -1.0 : fTmp;

			u1 = u0 + rad*Math.Sign(v0)*(Math.Acos(fTmp ));
			Dm0v0 = Dm0*v0;
		*/
		/*		CostDistIst = Math.Cos(Dist/AWG)*Math.Cos(Dm1/AWG) +
									Math.Sin(Dist/AWG)*Math.Sin(Dm1/AWG)*Math.Cos(Betta);
				CostDistIst = CostDistIst > 1.0 ? 1.0 : CostDistIst;
				CostDistIst = CostDistIst < -1.0 ? -1.0 : CostDistIst;

				DistIst = Math.Acos(CostDistIst);
				SinDistIst = Math.Sin(DistIst);
				Dist = DistIst*AWG;
		*/
		/*
			fTmp = Math.Sin(y0);
			fTmp1 = Math.Sin(y1);
			if(Math.Abs(Math.Cos(y1))<=Eps)
			{
				if(fTmp1 > 0.0)
					Az1 = Math.PI + x1 - x0;
				else
					Az1 = x0 - x1;
			}
			else
			{
		//		k = Math.Cos(y0)/Math.Cos(y1)*Math.Sqrt( (1.0-E2WG*fTmp1*fTmp1)/(1.0-E2WG*fTmp*fTmp)  );
				EWG = Math.Sqrt(E2WG);
				k = Math.Cos(y0)/Math.Cos(y1)*Math.Sqrt((1.0-EWG*fTmp1)*(1.0+EWG*fTmp1)/((1.0-EWG*fTmp)*(1.0-EWG*fTmp)));

				fTmp = Math.Sin(DegToRad(Az0))*k;
				fTmp = fTmp > 1.0? 1.0 : fTmp;
				fTmp = fTmp < -1.0? -1.0 : fTmp;

				Az1 = -Math.Asin (fTmp);
			}

			Az1 = RadToDeg(Mod2PI(Az1));
		*/

		/*
		//Ilyas
		public static double ReturnAntipodalDistanceIlyas(TGCoordinate Pt0, TGCoordinate Pt1, out double Az0, out double Az1)
		{
			bool antipod;
			long i;
			double Dd, Sigm;

			TGCoordinate CoordinateMiddle;
			double Azm0, Az0m, Azm1, Az1m;
			double Dm0, Dm1, y0, x0, y1, x1, dx, dy;
			double tx, ty;

			Pt0.X = Utils.Mod360(Pt0.X);
			Pt1.X = Utils.Mod360(Pt1.X);

			InverseProblem(Pt0, Pt1, out Dm1, out Azm1, out Az1m, out Sigm, out antipod);
			if (!antipod)
			{
				Az0 = Azm1;
				Az1 = Az1m;
				return Dm1;
			}

			double Rmin, oo = 0.01;

			CoordinateMiddle = DirectProblem(Pt0, Azm1, 0.5 * Dm1, out Azm0);
			//	InverseProblem(CoordinateMiddle, Pt1, Dm1, Azm1, Az1m, Sigm, antipod);

			InverseProblem(CoordinateMiddle, Pt0, out Dm0, out Azm0, out Az0m, out Sigm, out antipod);
			InverseProblem(CoordinateMiddle, Pt1, out Dm1, out Azm1, out Az1m, out Sigm, out antipod);

			Rmin = (Math.Abs(Azm1 - Azm0) - 180.0);

			if (Rmin == 0.0)
			{
				Az0 = Az0m;
				Az1 = Az1m;
				return Dm1 + Dm0;
			}

			tx = CoordinateMiddle.Y;
			x0 = tx + oo;
			if (x0 > 90.0)
				x0 = 90.0;
			else if (x0 < -90.0)
				x0 = -90.0;

			CoordinateMiddle.Y = x0;
			InverseProblem(CoordinateMiddle, Pt1, out Dm1, out Azm1, out Az1m, out Sigm, out antipod);
			InverseProblem(CoordinateMiddle, Pt0, out Dm0, out Azm0, out Az0m, out Sigm, out antipod);
			y0 = (Math.Abs(Azm1 - Azm0) - 180.0);

			if (y0 == 0.0)
			{
				Az0 = Az0m;
				Az1 = Az1m;
				return Dm1 + Dm0;
			}

			x1 = tx - oo;
			if (x1 > 90.0)
				x1 = 90.0;
			else if (x1 < -90.0)
				x1 = -90.0;

			CoordinateMiddle.Y = x1;
			InverseProblem(CoordinateMiddle, Pt1, out Dm1, out Azm1, out Az1m, out Sigm, out antipod);
			InverseProblem(CoordinateMiddle, Pt0, out Dm0, out Azm0, out Az0m, out  Sigm, out antipod);
			y1 = (Math.Abs(Azm1 - Azm0) - 180.0);

			if (y1 == 0.0)
			{
				Az0 = Az0m;
				Az1 = Az1m;
				return Dm1 + Dm0;
			}

			if (Rmin * y0 < 0.0)
			{
				x1 = tx; y1 = Rmin;
			}
			else if (Rmin * y1 < 0.0)
			{
				x0 = tx; y0 = Rmin;
			}
			else
			{
				if (Math.Abs(y1) > Math.Abs(Rmin) || Math.Abs(y0) > Math.Abs(Rmin))
				{
					if (Math.Abs(y1) > Math.Abs(y0))
					{
						x1 = tx; y1 = Rmin;
					}
					else
					{
						x0 = tx; y0 = Rmin;
					}
				}
			}

			i = 0;
			do
			{
				dx = x1 - x0; dy = y1 - y0;
				if (Math.Abs(dy) < Eps) break;

				Dd = tx;
				tx = x0 - y0 * dx / dy;

				CoordinateMiddle.Y = tx;
				InverseProblem(CoordinateMiddle, Pt1, out Dm1, out Azm1, out Az1m, out Sigm, out antipod);
				InverseProblem(CoordinateMiddle, Pt0, out Dm0, out Azm0, out Az0m, out Sigm, out antipod);

				ty = (Math.Abs(Azm1 - Azm0) - 180.0);
				if (ty == 0.0) break;

				if (ty * y0 < 0.0)
				{
					x1 = tx; y1 = ty;
				}
				else
				{
					x0 = tx; y0 = ty;
				}

				System.Windows.Forms.Application.DoEvents();
				if (breakFlg) break;
				i++;
			} while (Math.Abs(tx - Dd) > Eps);

			//char	Buff[128];
			//sprintf(Buff, "Iterations: %d", i);
			//MessageDlg(Buff, mtInformation, TMsgDlgButtons() << mbOK, 0);

			string msg = string.Format("Iterations: {0}", i);
			System.Windows.Forms.MessageBox.Show(msg, null, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

			Az0 = Az0m;
			Az1 = Az1m;
			return Dm1 + Dm0;
		}
		*/

		//Firdowsy
		public static double ReturnAntipodalDistance(TGCoordinate Pt0, TGCoordinate Pt1, out double Az0, out double Az1)
		{
			bool antipod;
			long i;
			double Dist, Sigm, AppSigm, Const;
			double AppAz0, AppAz1, Azm0, Az0m, Azm1, Az1m;

			double Dm0, Dm1;
			double u0, v0, u1, v1, Dm0v0, Dm1v1;
			double SinAlpha0, SinAlpha1, Betta, fTmp;

			TGCoordinate AppPt0, AppPt1;

			Pt0.X = Utils.Mod360(Pt0.X);
			Pt1.X = Utils.Mod360(Pt1.X);

			InverseProblem(Pt0, Pt1, out Dist, out Az0, out Az1, out Sigm, out antipod);
			if (!iterations)
				return Dist;


			AppPt0 = DirectProblem(Pt0, Az0, Dist, out AppAz0);
			u0 = Az0;

			InverseProblem(AppPt0, Pt1, out Dm0, out Az0m, out Azm0, out AppSigm, out antipod);

			Betta = Utils.DegToRad(AppAz0 - Az0m);
			v0 = Math.Sin(Betta);
			Dist -= Dm0 * Math.Cos(Betta);
			Betta = Math.Asin(v0);
			Dm0v0 = Dm0 * v0;

			u1 = Az0 + rad * Math.Sign(Math.Sin(Utils.DegToRad(Pt1.Y))) *
						Math.Abs(Betta * Math.Sin(AppSigm) /
						Math.Sin(Math.PI * (1.0 - FWG)));

			i = 0;
			Dm1 = fTmp = 0.0;

			do
			{
				AppPt1 = DirectProblem(Pt0, u1, Dist, out AppAz1);

				InverseProblem(AppPt1, Pt1, out Dm1, out Az1m, out Azm1, out AppSigm, out antipod);
				SinAlpha0 = Math.Sin(Utils.DegToRad(AppAz1 - Azm0));
				SinAlpha1 = Math.Sin(Utils.DegToRad(AppAz1 - Azm1));
				Betta = Utils.DegToRad(AppAz1 - Az1m);
				v1 = Math.Sin(Betta);
				Dist -= Dm1 * Math.Cos(Betta);

				Dm1v1 = Dm1 * v1;
				if (Math.Abs(Dm1v1) < Eps)
					break;

				Const = Math.Sign(SinAlpha0 * SinAlpha1) * Math.Abs(Dm0v0 / Dm1v1);	//zsign
				fTmp = u1;

				if (Math.Abs(1.0 - Const) > Eps)
					u1 = (u0 - Const * u1) / (1.0 - Const);
				else
					break;

				u0 = fTmp;
				fTmp = (u0 - u1);

				v0 = v1;
				Dm0v0 = Dm1v1;
				Azm0 = Azm1;
				i++;
				System.Windows.Forms.Application.DoEvents();
				if (breakFlg) break;
			}
			while ((Dm1 * Dm1 + fTmp * fTmp) >= Eps);

			//char[] Buff = new char[128];
			//sprintf(Buff, "Iterations: %d dL: %Le dAz: %Le", i, Dm1, fTmp);
			//MessageDlg(Buff, mtInformation, TMsgDlgButtons() << mbOK, 0);

			string msg = string.Format("Iterations: {0}\ndL: {1}\ndAz: {2}", i, Dm1, fTmp);
			System.Windows.Forms.MessageBox.Show(msg, null, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

			Az0 = Utils.Mod360(u1);
			Az1 = Utils.Mod360(AppAz1);
			return Dist;
		}
	}
}