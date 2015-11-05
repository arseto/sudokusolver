using System;
using noeg.Math;

namespace Sudoku
{
	class MainClass
	{		
		private static void PrintMatrix (int[][] v_Matrix)
		{
			for(int x = 0;x< v_Matrix.Length;x++)
			{
				string s = "";
				if(x==3||x==6){
					Console.WriteLine("-------------------");
				}
				for(int y=0;y< v_Matrix[x].Length;y++)
				{
					s+=v_Matrix[x][y].ToString() + " ";
					if(y==2||y==5) s+='|';
				}
				Console.WriteLine(s);
			}
		}

		public static void Main (string[] args)
		{
			bool verbose = false;
			if(args.Length>0) verbose = args[0].Equals("v");
			int i = 0;
			int[][] matrix = new int[9][];
			while(i<9)
			{
				string s = Console.ReadLine();
				string[] line = s.Split(' ');
				if(line.Length==9)
				{
					int[] intArr = new int[9];
					for(int j=0;j<9;j++)
					{
						int x = int.Parse(line[j]);
						intArr[j]=x;
					}
					//sud.AddRow(intArr);
					matrix[i] = intArr;		
					i++;
				}
				else
				{
					//invalid, please retype
					Console.WriteLine("invalid line, please retype");
					continue;
				}
				
			}
			Console.WriteLine();
			PrintMatrix(matrix);
			
			DateTime startTime = DateTime.Now;
			Console.WriteLine ("Started: {0}", startTime);
			
			//SudokuSolver_old solver = new SudokuSolver_old(sud);
			//solver.Iterate(false);
			SudokuSolver solver = new SudokuSolver(matrix, verbose);
			solver.Solve();


			DateTime stopTime = DateTime.Now;
			Console.WriteLine ("Stopped: {0}", stopTime);
			Console.WriteLine ("solution generated: {0}", solver.possibleSolutions.Count);
			Console.WriteLine ("backtracks: {0}", solver.deadendCount);

			foreach(int[][] solution in solver.possibleSolutions)
			{
				PrintMatrix(solution);
			}
			Console.WriteLine();
			
			
			TimeSpan elapsedTime = stopTime - startTime;
			Console.WriteLine ("Elapsed: {0}", elapsedTime);
			Console.WriteLine ("in hours       :" + elapsedTime.TotalHours);
			Console.WriteLine ("in minutes     :" + elapsedTime.TotalMinutes);
			Console.WriteLine ("in seconds     :" + elapsedTime.TotalSeconds);
			Console.WriteLine ("in milliseconds:" + elapsedTime.TotalMilliseconds);

		}
	
		
		private static void PrintMatrixStr (string[][] v_Matrix)
		{
			for(int x = 0;x< v_Matrix.Length;x++)
			{
				string s = "";
				if(x==3||x==6){
					Console.WriteLine("-------------------");
				}
				for(int y=0;y< v_Matrix[x].Length;y++)
				{
					s+=v_Matrix[x][y].ToString() + " ";
					if(y==2||y==5) s+='|';
				}
				Console.WriteLine(s);
			}
		}
	}
}

