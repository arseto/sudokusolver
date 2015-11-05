using System;
using System.Collections.Generic;
using System.Text;

namespace noeg.Math
{
    public class SudokuSolver
    {
        int[][] matrix;
        //public int[][] solution { get; set; }
		public List<int[][]> possibleSolutions{get;set;}
		public int deadendCount;
		bool verbose=false;
        public bool isComplete(int[][] v_Matrix)
        {
            if (v_Matrix == null) return false;
            //bool complete = true;
            for (int i = 0; i < v_Matrix.Length; i++)
            {
                if (v_Matrix[i] == null) return false;
                for (int j = 0; j < v_Matrix[i].Length; j++)
                {
                    if (v_Matrix[i][j] == 0) return false;
                }
            }
            return true;
        }
        public SudokuSolver(int[][] v_Matrix, bool v_bVerbose)
        {
			this.deadendCount = 0;
            this.matrix = array_deep_copy(v_Matrix);
			this.verbose = v_bVerbose;
			this.possibleSolutions = new List<int[][]>();
        }
        public bool Solve()
        {
            return Solve2(this.matrix);
        }
        public bool Solve(int[][] v_Matrix)
        {
            int progress = 1;
            while (progress>0)
            {
                progress = 0;
                int _leastBranchRow = -1;
                int _leastBranchCol = -1;
                List<int> _leastBranch = new List<int>();
                for (int i = 0; i < v_Matrix.Length; i++)
                {
                    for (int j = 0; j < v_Matrix[i].Length; j++)
                    {
                        if (v_Matrix[i][j] > 0) continue;//skip if value is not zero
                        _leastBranch = GetAllowedValues(v_Matrix, i, j);
                        _leastBranchRow = i;
                        _leastBranchCol = j;
                        if (_leastBranch.Count == 1)
                        {
                            //assign value
                            v_Matrix[i][j] = _leastBranch[0];
                            if(verbose) Console.WriteLine("set:[{0},{1}]:{2}", i, j, _leastBranch[0]);
                            progress++;
                        }
                    }
                }
                if (_leastBranch.Count == 0 && progress==0&& !isComplete(v_Matrix))
                {
                    //deadend
                    if(verbose) Console.WriteLine("Deadend");
					deadendCount++;
                    return false;
                }
                else if(_leastBranch.Count>1 && progress ==0)
                {
                    bool solve = false;
                    //try each branch
                    foreach (int branch in _leastBranch)
                    {
                        int[][] newMatrix = array_deep_copy(v_Matrix);
                        newMatrix[_leastBranchRow][_leastBranchCol] = branch;
                        if(verbose) Console.WriteLine("guess:[{0},{1}]:{2}", _leastBranchRow, _leastBranchCol, branch);
                        solve = solve || Solve(newMatrix);
                        //if (solve) return true;
                    }
                    progress++;
                    return solve;
                }
                if (isComplete(v_Matrix))
                {
                    //solution = v_Matrix;
					possibleSolutions.Add(array_deep_copy(v_Matrix));
					if(true)
					{
						Console.WriteLine("Solution found");
					}
                    return true;
                }
            }
            return false;
        }
        public bool Solve2(int[][] v_Matrix)
        {
            int progress = 1;
            while (progress > 0)
            {
                progress = 0;
                int _leastBranchRow = -1;
                int _leastBranchCol = -1;
                List<int> _leastBranch = new List<int>();
                for (int i = 0; i < v_Matrix.Length; i++)
                {
                    for (int j = 0; j < v_Matrix[i].Length; j++)
                    {
                        if (v_Matrix[i][j] > 0) continue;//skip if value is not zero
                        _leastBranch = GetAllowedValues(v_Matrix, i, j);
                        _leastBranchRow = i;
                        _leastBranchCol = j;
                        if (_leastBranch.Count == 1)
                        {
                            //assign value
                            v_Matrix[i][j] = _leastBranch[0];
                            if (verbose) Console.WriteLine("set:[{0},{1}]:{2}", i, j, _leastBranch[0]);
                            progress++;
                        }
                    }
                }
                if (_leastBranch.Count > 1 && progress == 0)
                {
                    bool solve = false;
                    //try each branch
                    foreach (int branch in _leastBranch)
                    {
                        int[][] newMatrix = array_deep_copy(v_Matrix);
                        newMatrix[_leastBranchRow][_leastBranchCol] = branch;
                        if (verbose) Console.WriteLine("guess:[{0},{1}]:{2}", _leastBranchRow, _leastBranchCol, branch);
                        progress++;
                        solve = solve || Solve2(newMatrix);
                        //if (solve) return true;
                    }
                    return solve;
                }
                if (isComplete(v_Matrix))
                {
                    //solution = v_Matrix;
                    possibleSolutions.Add(array_deep_copy(v_Matrix));
                    if (true)
                    {
                        Console.WriteLine("Solution found");
                    }
                    return true;
                }
                if (_leastBranch.Count == 0 && progress == 0)
                {
                    //deadend
                    if (verbose) Console.WriteLine("Deadend");
                    deadendCount++;
                    return false;
                }
            }
            return false;
        }
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
        
        

        private static List<int> GetAllowedValues(int[][] v_Matrix, int v_Row, int v_Col)
        {
            List<int> temp1 = GetAllowedValuesInColumn(v_Matrix, v_Col);
            List<int> temp2 = GetAllowedValuesInRow(v_Matrix, v_Row, temp1);
            List<int> _zone = GetAllowedValuesInZone(v_Matrix, v_Row, v_Col);
            List<int> temp3 = new List<int>();
            foreach (int i in temp2)
            {
                if (!_zone.Contains(i))
                    temp3.Add(i);
            }
            return temp3;
        }

        private static List<int> GetAllowedValuesInZone(int[][] v_Matrix, int v_Row, int v_Col)
        {
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
                    if (v_Matrix[i][j] > 0)
                        _zone.Add(v_Matrix[i][j]);
                }
            }
            return _zone;
        }

        private static List<int> GetAllowedValuesInRow(int[][] v_Matrix, int v_Row, List<int> temp1)
        {
            List<int> temp2 = new List<int>();
            for (int i = 0; i < temp1.Count; i++)
            {
                if (!array_contains(v_Matrix[v_Row], temp1[i]))
                    temp2.Add(temp1[i]);
            }
            return temp2;
        }

        private static List<int> GetAllowedValuesInColumn(int[][] v_Matrix, int v_Col)
        {
            List<int> temp1 = new List<int>();
            for (int i = 1; i <= 9; i++)
            {
                int[][] _transpose = transpose(v_Matrix);
                if (!array_contains(_transpose[v_Col], i))
                    temp1.Add(i);
            }
            return temp1;
        }
		private static bool array_contains(int[] v_array, int v_item)
		{
			foreach(int i in v_array)
			{
				if(i.Equals(v_item))return true;
			}
			return false;
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
    }
	
}