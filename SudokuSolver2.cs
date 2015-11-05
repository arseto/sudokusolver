using System;
using System.Collections.Generic;
namespace noeg.Math
{
	public class SudokuSolver2
	{
		public List<string[][]> PossibleSolutions {get;set;}
		public List<int[][]> Solve()
		{
			return new List<int[][]>();
		}
		void RecursiveSolver(SolverMatrix matrix)
		{
			
		}
	}
	public class SolverMatrix
	{
		string[][] solverMatrix {get;set;}
		public int[] nextGuessValues {get;set;}
		public int nextGuessRow {get;set;}
		public int nextGuessCell {get;set;}
		public bool IsSolved {get;set;}
		
		public SolverMatrix(int[][] v_2DMatrix)
		{
			solverMatrix = new string[9][];
			IsSolved = true;
			for (int row=0;row<v_2DMatrix.Length;row++)
			{
				solverMatrix[row] = new string[9];
				for (int cell=0;cell<v_2DMatrix[row].Length;cell++)
				{
					if(v_2DMatrix[row][cell] > 0)
					{
						//Console.ReadKey();
						solverMatrix[row][cell] = v_2DMatrix[row][cell].ToString();
					}
					else
					{
						solverMatrix[row][cell] = GetAllowedValues(v_2DMatrix, row, cell);
						IsSolved=false;
					}
				}
			}
		}
		
		
		private string GetAllowedValues(int[][] v_Matrix, int v_Row, int v_Col)
        {
            //List<int> result = new List<int>();
            List<int> temp1 = new List<int>();
            List<int> temp2 = new List<int>();
            List<int> temp3 = new List<int>();
            //allowed values in column
            for (int i = 1; i <= 9; i++)
            {
                int[][] _transpose = transpose(v_Matrix);               
				if(!array_contains(_transpose[v_Col], i))
					temp1.Add(i);
            }
            //allowed values in row
            for (int i = 0; i < temp1.Count;i++ )
            {
				if(!array_contains(v_Matrix[v_Row], temp1[i]))
					temp2.Add(temp1[i]);
            }
            //allowed values in 3x3 zone
            List<int> _zone = new List<int>();
            //1: determine boundary
            int startRow = v_Row - (v_Row % 3);//endrow is startrow+2
            int startCol = v_Col - (v_Col % 3);//endcol is startcol+2
            //2: get values from 3x3 submatrix in defined boundary
            for (int i = startRow; i < startRow + 3; i++)
            {
                for (int j = startCol; j < startCol + 3; j++)
                {
                    if(v_Matrix[i][j]>0)
                        _zone.Add(v_Matrix[i][j]);
                }
            }
            foreach (int i in temp2)
            {
                if (!_zone.Contains(i))
                    temp3.Add(i);
            }
			string hsl = "";
            foreach(int item in temp3)
			{
				hsl+=item.ToString();
			}
        	return hsl;
        }
		void SetValue(int v_Row, int v_Col, int v_Value)
		{
			solverMatrix[v_Col][v_Row] = v_Value.ToString();
		}
		void RemoveFromRow(int v_Row, int v_Value)
		{
			foreach(string cell in solverMatrix[v_Row])
			{
				if(cell.Contains(v_Value.ToString()))
				{
					cell.Replace(v_Value.ToString(), "");
				}
			}
		}
		void RemoveFromCol(int v_Col, int v_Value)
		{
			for(int row=0;row<solverMatrix.Length;row++)
			{
				for(int cell=0;cell<solverMatrix[row].Length;cell++)
				{
					if(cell==v_Col)
					{
						if(solverMatrix[row][cell].Contains(v_Value.ToString()))
						{
							solverMatrix[row][cell].Replace(v_Value.ToString(), "");
						}
					}
				}
			}
		}
		void RemoveFromZone(int v_ZoneX, int v_ZoneY, int v_Value)
		{
			//1: determine boundary
            int startRow = v_ZoneX*3;//endrow is startrow+2
            int startCol = v_ZoneY*3;//endcol is startcol+2
            //2: remove values from 3x3 submatrix in defined boundary
            for (int i = startRow; i < startRow + 3; i++)
            {
                for (int j = startCol; j < startCol + 3; j++)
                {
                 	if(solverMatrix[i][j].Contains(v_Value.ToString()))
					{
						solverMatrix[i][j].Replace(v_Value.ToString(), "");
					}
                }
            }
		}
		public void Guess(int v_Row, int v_Col, int v_Value)
		{
			SetValue(v_Row, v_Col, v_Value);
			RemoveFromRow(v_Row, v_Value);
			RemoveFromCol(v_Col, v_Value);
			RemoveFromZone(v_Row - (v_Row % 3),v_Col - (v_Col % 3), v_Value);
		}
		private static int[][] transpose(int[][] v_Matrix)
        {
            int[][] result = new int[9][];
            for (int i = 0; i < v_Matrix.Length; i++)
            {
                
                for (int j = 0; j < v_Matrix[i].Length; j++)
                {
                    if (result[j] == null) result[j] = new int[9];
                    result[j][i] = v_Matrix[i][j];
                }
            }
            return result;
        }
		private static bool array_contains(int[] v_array, int v_item)
		{
			foreach(int i in v_array)
			{
				if(i.Equals(v_item))return true;
			}
			return false;
		}
		int[][] array_deep_copy(int[][] v_matrix)
        {
            int[][] result = new int[v_matrix.Length][];
            for(int i = 0;i<v_matrix.Length;i++)
            {
                result[i] = new int[v_matrix[i].Length];
                for (int j = 0;j<v_matrix[i].Length;j++)
                {
                    result[i][j] = v_matrix[i][j];
                }
            }
            return result;
        }
		public string[][] ReadTempMatrix()
		{
			string[][] result = new string[9][];
			for (int row=0;row<solverMatrix.Length;row++)
			{
				result[row]=new string[9];
				for(int cell=0;cell<solverMatrix[row].Length;cell++)
				{
					
					for(int probability=0;probability<solverMatrix[row][cell].Length;probability++)
					{
						result[row][cell]+=solverMatrix[row][cell][probability].ToString();	
					}
				}
			}
			return result;
		}
	}
}

