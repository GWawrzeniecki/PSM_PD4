using System;
namespace PSM_PD4.Gauss
{
    public class GaussElimination
    {
        public GaussElimination()
        {

        }

        // Solve the system of equations.
        public static string cmdSolve_Click(int[][] values, int[] results)
        {
            const double tiny = 0.00001;
            string txt = "";

            // Build the augmented matrix.
            // The values num_rows and num_cols are the number of rows
            // and columns in the matrix, not the augmented matrix.
            int num_rows, num_cols;
            double[,] arr = LoadArray(out num_rows, out num_cols,values, results);
            double[,] orig_arr = LoadArray(out num_rows, out num_cols, values, results);

            // Display the initial arrays.
            //PrintArray(arr);
            //PrintArray(orig_arr);

            // Start solving.
            for (int r = 0; r < num_rows - 1; r++)
            {
                // Zero out all entries in column r after this row.
                // See if this row has a non-zero entry in column r.
                if (Math.Abs(arr[r, r]) < tiny)
                {
                    // Too close to zero. Try to swap with a later row.
                    for (int r2 = r + 1; r2 < num_rows; r2++)
                    {
                        if (Math.Abs(arr[r2, r]) > tiny)
                        {
                            // This row will work. Swap them.
                            for (int c = 0; c <= num_cols; c++)
                            {
                                double tmp = arr[r, c];
                                arr[r, c] = arr[r2, c];
                                arr[r2, c] = tmp;
                            }
                            break;
                        }
                    }
                }

                // If this row has a non-zero entry in column r, use it.
                if (Math.Abs(arr[r, r]) > tiny)
                {
                    // Zero out this column in later rows.
                    for (int r2 = r + 1; r2 < num_rows; r2++)
                    {
                        double factor = -arr[r2, r] / arr[r, r];
                        for (int c = r; c <= num_cols; c++)
                        {
                            arr[r2, c] = arr[r2, c] + factor * arr[r, c];
                        }
                    }
                }
            }

            // Display the upper-triangular array.
            //PrintArray(arr);

            // See if we have a solution.
            if (arr[num_rows - 1, num_cols - 1] == 0)
            {
                // We have no solution.
                // See if all of the entries in this row are 0.
                bool all_zeros = true;
                for (int c = 0; c <= num_cols + 1; c++)
                {
                    if (arr[num_rows - 1, c] != 0)
                    {
                        all_zeros = false;
                        break;
                    }
                }
                if (all_zeros)
                {
                    txt = "The solution is not unique";
                }
                else
                {
                    txt = "There is no solution";
                }
            }
            else
            {
                // Backsolve.
                for (int r = num_rows - 1; r >= 0; r--)
                {
                    double tmp = arr[r, num_cols];
                    for (int r2 = r + 1; r2 < num_rows; r2++)
                    {
                        tmp -= arr[r, r2] * arr[r2, num_cols + 1];
                    }
                    arr[r, num_cols + 1] = tmp / arr[r, r];
                }

                // Display the results.
                txt = "       Values:";
                for (int r = 0; r < num_rows; r++)
                {
                    txt += "\r\nx" + r.ToString() + " = " +
                        arr[r, num_cols + 1].ToString();
                }

                // Verify.
                txt += "\r\n    Check:";
                for (int r = 0; r < num_rows; r++)
                {
                    double tmp = 0;
                    for (int c = 0; c < num_cols; c++)
                    {
                        tmp += orig_arr[r, c] * arr[c, num_cols + 1];
                    }
                    txt += "\r\n" + tmp.ToString();
                }

                txt = txt.Substring("\r\n".Length + 1);
            }

            return txt;
        }

        // Load the augmented array.
        // Column num_cols holds the result values.
        // Column num_cols + 1 will hold the variables'
        // final values after backsolving.
        public static double[,] LoadArray(out int num_rows, out int num_cols, int[][] values, int[] results)
        {
            // Build the augmented matrix.
            //string[] value_rows = results.Split(
            //    new string[] { "\n", " " },
            //    StringSplitOptions.RemoveEmptyEntries);
            //string[] coef_rows = values.Split(
            //    new string[] { "\n" },
            //    StringSplitOptions.RemoveEmptyEntries);
            //string[] one_row = coef_rows[0].Split(
            //    new string[] { " " },
            //    StringSplitOptions.RemoveEmptyEntries);

            var one_row = values[0];

            num_rows = values.GetUpperBound(0) + 1;
            num_cols = one_row.GetUpperBound(0) + 1;
            double[,] arr = new double[num_rows, num_cols + 2];
            for (int r = 0; r < num_rows; r++)
            {
                one_row = values[r];
                for (int c = 0; c < num_cols; c++)
                {
                    arr[r, c] = one_row[c];
                }
                arr[r, num_cols] = results[r];
            }

            return arr;
        }
    }
}
