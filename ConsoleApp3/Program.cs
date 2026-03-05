using System;
using System.Text;

namespace MatrixCalculator {

  // Custom exception classes
  public class MatrixException : Exception {

    public MatrixException(string message) : base(message) { }

  }

  public class MatrixDimensionException : MatrixException {

    public MatrixDimensionException(string message) : base(message) { }

  }

  public class MatrixSingularException : MatrixException {

    public MatrixSingularException() : base("Matrix is singular and cannot be inverted.") { }

  }

  public class SquareMatrix : ICloneable, IComparable<SquareMatrix> {

    private double[,] _matrix;
    private int _size;

    // Properties
    public int Size => _size;
    
    public double this[int rowIndex, int colIndex] {
      get {
        if (rowIndex < 0 || rowIndex >= _size || colIndex < 0 || colIndex >= _size) {
          throw new IndexOutOfRangeException("Matrix indices out of range.");
        }
        return _matrix[rowIndex, colIndex];
      }
      set {
        if (rowIndex < 0 || rowIndex >= _size || colIndex < 0 || colIndex >= _size) {
          throw new IndexOutOfRangeException("Matrix indices out of range.");
        }
        _matrix[rowIndex, colIndex] = value;
      }
    }

    // Constructors
    public SquareMatrix(int size) {
      if (size <= 0) {
        throw new ArgumentException("Matrix size must be positive.");
      }
      _size = size;
      _matrix = new double[size, size];
    }

    public SquareMatrix(int size, double minValue, double maxValue) : this(size) {
      Random randomGenerator = new Random();
      for (int rowIndex = 0; rowIndex < size; ++rowIndex) {
        for (int colIndex = 0; colIndex < size; ++colIndex) {
          _matrix[rowIndex, colIndex] = randomGenerator.NextDouble() * (maxValue - minValue) + minValue;
        }
      }
    }

    public SquareMatrix(double[,] sourceData) {
      if (sourceData == null) {
        throw new ArgumentNullException(nameof(sourceData));
      }
      if (sourceData.GetLength(0) != sourceData.GetLength(1)) {
        throw new MatrixDimensionException("Array is not square.");
      }
      
      _size = sourceData.GetLength(0);
      _matrix = new double[_size, _size];
      
      for (int rowIndex = 0; rowIndex < _size; ++rowIndex) {
        for (int colIndex = 0; colIndex < _size; ++colIndex) {
          _matrix[rowIndex, colIndex] = sourceData[rowIndex, colIndex];
        }
      }
    }

    // Private constructor for cloning
    private SquareMatrix(SquareMatrix sourceMatrix) {
      _size = sourceMatrix._size;
      _matrix = new double[_size, _size];
      
      for (int rowIndex = 0; rowIndex < _size; ++rowIndex) {
        for (int colIndex = 0; colIndex < _size; ++colIndex) {
          _matrix[rowIndex, colIndex] = sourceMatrix._matrix[rowIndex, colIndex];
        }
      }
    }

    // Operator + overloading
    public static SquareMatrix operator +(SquareMatrix leftMatrix, SquareMatrix rightMatrix) {
      if (leftMatrix == null || rightMatrix == null) {
        throw new ArgumentNullException("Matrix cannot be null.");
      }
      if (leftMatrix._size != rightMatrix._size) {
        throw new MatrixDimensionException("Matrices dimensions must match for addition.");
      }

      SquareMatrix resultMatrix = new SquareMatrix(leftMatrix._size);
      
      for (int rowIndex = 0; rowIndex < leftMatrix._size; ++rowIndex) {
        for (int colIndex = 0; colIndex < leftMatrix._size; ++colIndex) {
          resultMatrix._matrix[rowIndex, colIndex] = leftMatrix._matrix[rowIndex, colIndex] + rightMatrix._matrix[rowIndex, colIndex];
        }
      }
      
      return resultMatrix;
    }

    // Operator * overloading (matrix multiplication)
    public static SquareMatrix operator *(SquareMatrix leftMatrix, SquareMatrix rightMatrix) {
      if (leftMatrix == null || rightMatrix == null) {
        throw new ArgumentNullException("Matrix cannot be null.");
      }
      if (leftMatrix._size != rightMatrix._size) {
        throw new MatrixDimensionException("Matrices dimensions must match for multiplication.");
      }

      SquareMatrix resultMatrix = new SquareMatrix(leftMatrix._size);
      
      for (int rowIndex = 0; rowIndex < leftMatrix._size; ++rowIndex) {
        for (int colIndex = 0; colIndex < leftMatrix._size; ++colIndex) {
          double sumValue = 0;
          for (int kIndex = 0; kIndex < leftMatrix._size; ++kIndex) {
            sumValue += leftMatrix._matrix[rowIndex, kIndex] * rightMatrix._matrix[kIndex, colIndex];
          }
          resultMatrix._matrix[rowIndex, colIndex] = sumValue;
        }
      }
      
      return resultMatrix;
    }

    // Operator * overloading (scalar multiplication)
    public static SquareMatrix operator *(SquareMatrix matrix, double scalarValue) {
      if (matrix == null) {
        throw new ArgumentNullException(nameof(matrix));
      }

      SquareMatrix resultMatrix = new SquareMatrix(matrix._size);
      
      for (int rowIndex = 0; rowIndex < matrix._size; ++rowIndex) {
        for (int colIndex = 0; colIndex < matrix._size; ++colIndex) {
          resultMatrix._matrix[rowIndex, colIndex] = matrix._matrix[rowIndex, colIndex] * scalarValue;
        }
      }
      
      return resultMatrix;
    }

    public static SquareMatrix operator *(double scalarValue, SquareMatrix matrix) {
      return matrix * scalarValue;
    }

    // Comparison operators
    public static bool operator >(SquareMatrix leftMatrix, SquareMatrix rightMatrix) {
      if (leftMatrix == null || rightMatrix == null) {
        throw new ArgumentNullException("Matrix cannot be null.");
      }
      return leftMatrix.CompareTo(rightMatrix) > 0;
    }

    public static bool operator <(SquareMatrix leftMatrix, SquareMatrix rightMatrix) {
      if (leftMatrix == null || rightMatrix == null) {
        throw new ArgumentNullException("Matrix cannot be null.");
      }
      return leftMatrix.CompareTo(rightMatrix) < 0;
    }

    public static bool operator >=(SquareMatrix leftMatrix, SquareMatrix rightMatrix) {
      if (leftMatrix == null || rightMatrix == null) {
        throw new ArgumentNullException("Matrix cannot be null.");
      }
      return leftMatrix.CompareTo(rightMatrix) >= 0;
    }

    public static bool operator <=(SquareMatrix leftMatrix, SquareMatrix rightMatrix) {
      if (leftMatrix == null || rightMatrix == null) {
        throw new ArgumentNullException("Matrix cannot be null.");
      }
      return leftMatrix.CompareTo(rightMatrix) <= 0;
    }

    public static bool operator ==(SquareMatrix leftMatrix, SquareMatrix rightMatrix) {
      if (ReferenceEquals(leftMatrix, null) && ReferenceEquals(rightMatrix, null)) {
        return true;
      }
      if (ReferenceEquals(leftMatrix, null) || ReferenceEquals(rightMatrix, null)) {
        return false;
      }
      return leftMatrix.Equals(rightMatrix);
    }

    public static bool operator !=(SquareMatrix leftMatrix, SquareMatrix rightMatrix) {
      return !(leftMatrix == rightMatrix);
    }

    // true/false operators
    public static bool operator true(SquareMatrix matrix) {
      if (matrix == null) {
        return false;
      }
      return matrix.CalculateDeterminant() != 0;
    }

    public static bool operator false(SquareMatrix matrix) {
      if (matrix == null) {
        return true;
      }
      return matrix.CalculateDeterminant() == 0;
    }

    // Type casting operators
    public static explicit operator double[,](SquareMatrix matrix) {
      if (matrix == null) {
        throw new ArgumentNullException(nameof(matrix));
      }
      
      double[,] resultArray = new double[matrix._size, matrix._size];
      
      for (int rowIndex = 0; rowIndex < matrix._size; ++rowIndex) {
        for (int colIndex = 0; colIndex < matrix._size; ++colIndex) {
          resultArray[rowIndex, colIndex] = matrix._matrix[rowIndex, colIndex];
        }
      }
      
      return resultArray;
    }

    public static implicit operator SquareMatrix(double[,] sourceArray) {
      if (sourceArray == null) {
        throw new ArgumentNullException(nameof(sourceArray));
      }
      return new SquareMatrix(sourceArray);
    }

    // Method to calculate determinant
    public double CalculateDeterminant() {
      if (_size == 1) {
        return _matrix[0, 0];
      }
      
      if (_size == 2) {
        return _matrix[0, 0] * _matrix[1, 1] - _matrix[0, 1] * _matrix[1, 0];
      }

      double determinantValue = 0;
      
      for (int colIndex = 0; colIndex < _size; ++colIndex) {
        SquareMatrix minorMatrix = this.CreateMinorMatrix(0, colIndex);
        double signValue = (colIndex % 2 == 0) ? 1 : -1;
        determinantValue += signValue * _matrix[0, colIndex] * minorMatrix.CalculateDeterminant();
      }
      
      return determinantValue;
    }

    // Method to calculate inverse matrix
    public SquareMatrix CalculateInverse() {
      double determinantValue = this.CalculateDeterminant();
      
      if (Math.Abs(determinantValue) < 1e-10) {
        throw new MatrixSingularException();
      }

      SquareMatrix resultMatrix = new SquareMatrix(_size);

      if (_size == 1) {
        resultMatrix._matrix[0, 0] = 1.0 / _matrix[0, 0];
        return resultMatrix;
      }

      for (int rowIndex = 0; rowIndex < _size; ++rowIndex) {
        for (int colIndex = 0; colIndex < _size; ++colIndex) {
          SquareMatrix minorMatrix = this.CreateMinorMatrix(rowIndex, colIndex);
          double signValue = ((rowIndex + colIndex) % 2 == 0) ? 1 : -1;
          resultMatrix._matrix[colIndex, rowIndex] = signValue * minorMatrix.CalculateDeterminant() / determinantValue;
        }
      }
      
      return resultMatrix;
    }

    // Helper method to create minor matrix
    private SquareMatrix CreateMinorMatrix(int excludedRow, int excludedCol) {
      SquareMatrix resultMatrix = new SquareMatrix(_size - 1);
      int targetRowIndex = 0;
      
      for (int sourceRowIndex = 0; sourceRowIndex < _size; ++sourceRowIndex) {
        if (sourceRowIndex == excludedRow) {
          continue;
        }
        
        int targetColIndex = 0;
        for (int sourceColIndex = 0; sourceColIndex < _size; ++sourceColIndex) {
          if (sourceColIndex == excludedCol) {
            continue;
          }
          
          resultMatrix._matrix[targetRowIndex, targetColIndex] = _matrix[sourceRowIndex, sourceColIndex];
          ++targetColIndex;
        }
        ++targetRowIndex;
      }
      
      return resultMatrix;
    }

    // ToString() implementation
    public override string ToString() {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine($"Square Matrix {_size}x{_size}:");
      
      for (int rowIndex = 0; rowIndex < _size; ++rowIndex) {
        stringBuilder.Append("[ ");
        for (int colIndex = 0; colIndex < _size; ++colIndex) {
          stringBuilder.Append($"{_matrix[rowIndex, colIndex],8:F2} ");
        }
        stringBuilder.AppendLine("]");
      }
      
      return stringBuilder.ToString();
    }

    // CompareTo() implementation
    public int CompareTo(SquareMatrix otherMatrix) {
      if (otherMatrix == null) {
        return 1;
      }
      
      double thisDeterminant = this.CalculateDeterminant();
      double otherDeterminant = otherMatrix.CalculateDeterminant();
      
      return thisDeterminant.CompareTo(otherDeterminant);
    }

    // Equals() implementation
    public override bool Equals(object comparedObject) {
      if (comparedObject == null || this.GetType() != comparedObject.GetType()) {
        return false;
      }
      
      SquareMatrix otherMatrix = (SquareMatrix)comparedObject;
      
      if (this._size != otherMatrix._size) {
        return false;
      }
      
      for (int rowIndex = 0; rowIndex < _size; ++rowIndex) {
        for (int colIndex = 0; colIndex < _size; ++colIndex) {
          if (Math.Abs(this._matrix[rowIndex, colIndex] - otherMatrix._matrix[rowIndex, colIndex]) > 1e-10) {
            return false;
          }
        }
      }
      
      return true;
    }

    // GetHashCode() implementation
    public override int GetHashCode() {
      int hashCodeValue = 17;
      hashCodeValue = hashCodeValue * 23 + _size.GetHashCode();
      
      for (int rowIndex = 0; rowIndex < _size; ++rowIndex) {
        for (int colIndex = 0; colIndex < _size; ++colIndex) {
          hashCodeValue = hashCodeValue * 23 + _matrix[rowIndex, colIndex].GetHashCode();
        }
      }
      
      return hashCodeValue;
    }

    // Prototype pattern implementation (deep copy)
    public object Clone() {
      return new SquareMatrix(this);
    }

    public SquareMatrix DeepCopy() {
      return (SquareMatrix)this.Clone();
    }

  }

  // Test application "Matrix Calculator"
  class Program {

    static void Main(string[] args) {
      string headerMessage = "==========================================\n" +
                           "     MATRIX CALCULATOR DEMO\n" +
                           "==========================================";
      Console.WriteLine(headerMessage);

      try {
        // Demo of matrix creation
        string creationHeader = "\n--- Creating random matrices ---";
        Console.WriteLine(creationHeader);
        
        SquareMatrix firstMatrix = new SquareMatrix(3, -10, 10);
        SquareMatrix secondMatrix = new SquareMatrix(3, -5, 5);
        
        string firstMatrixOutput = "First Matrix:\n" + firstMatrix.ToString();
        string secondMatrixOutput = "\nSecond Matrix:\n" + secondMatrix.ToString();
        Console.Write(firstMatrixOutput + secondMatrixOutput);

        // Demo of addition operator
        string additionHeader = "\n--- Matrix Addition ---";
        Console.WriteLine(additionHeader);
        
        SquareMatrix sumMatrix = firstMatrix + secondMatrix;
        string additionOutput = "First + Second:\n" + sumMatrix.ToString();
        Console.Write(additionOutput);

        // Demo of multiplication operator
        string multiplicationHeader = "\n--- Matrix Multiplication ---";
        Console.WriteLine(multiplicationHeader);
        
        SquareMatrix productMatrix = firstMatrix * secondMatrix;
        string multiplicationOutput = "First * Second:\n" + productMatrix.ToString();
        Console.Write(multiplicationOutput);

        // Demo of scalar multiplication
        string scalarHeader = "\n--- Scalar Multiplication ---";
        Console.WriteLine(scalarHeader);
        
        SquareMatrix scalarProduct = firstMatrix * 2.5;
        string scalarOutput = "First * 2.5:\n" + scalarProduct.ToString();
        Console.Write(scalarOutput);

        // Demo of determinant calculation
        string determinantHeader = "\n--- Determinant Calculation ---";
        Console.WriteLine(determinantHeader);
        
        double firstDeterminant = firstMatrix.CalculateDeterminant();
        double secondDeterminant = secondMatrix.CalculateDeterminant();
        
        string determinantOutput = $"Determinant of First Matrix: {firstDeterminant:F4}\n" +
                                 $"Determinant of Second Matrix: {secondDeterminant:F4}";
        Console.WriteLine(determinantOutput);

        // Demo of comparison operators
        string comparisonHeader = "\n--- Comparison Operators ---";
        Console.WriteLine(comparisonHeader);
        
        string comparisonOutput = $"First > Second : {firstMatrix > secondMatrix}\n" +
                                $"First < Second : {firstMatrix < secondMatrix}\n" +
                                $"First == Second: {firstMatrix == secondMatrix}\n" +
                                $"First != Second: {firstMatrix != secondMatrix}";
        Console.WriteLine(comparisonOutput);

        // Demo of true/false operators
        string trueFalseHeader = "\n--- True/False Operators ---";
        Console.WriteLine(trueFalseHeader);
        
        string firstStatus = firstMatrix ? "non-singular" : "singular";
        string secondStatus = secondMatrix ? "non-singular" : "singular";
        string trueFalseOutput = $"First is {firstStatus}\n" +
                               $"Second is {secondStatus}";
        Console.WriteLine(trueFalseOutput);

        // Demo of inverse matrix calculation (if possible)
        string inverseHeader = "\n--- Inverse Matrix Calculation ---";
        Console.WriteLine(inverseHeader);
        
        try {
          if (firstMatrix) {
            SquareMatrix inverseMatrix = firstMatrix.CalculateInverse();
            string inverseOutput = "Inverse of First Matrix:\n" + inverseMatrix.ToString();
            Console.Write(inverseOutput);
            
            // Verification: A * A^(-1) should be close to identity matrix
            SquareMatrix identityCheck = firstMatrix * inverseMatrix;
            string identityOutput = "First * Inverse (should be close to identity):\n" + identityCheck.ToString();
            Console.Write(identityOutput);
          } else {
            string singularMessage = "First matrix is singular, cannot compute inverse.";
            Console.WriteLine(singularMessage);
          }
        } catch (MatrixSingularException exception) {
          string errorMessage = $"Error: {exception.Message}";
          Console.WriteLine(errorMessage);
        }

        // Demo of type casting
        string castingHeader = "\n--- Type Casting ---";
        Console.WriteLine(castingHeader);
        
        double[,] firstArray = (double[,])firstMatrix;
        StringBuilder castingBuilder = new StringBuilder();
        castingBuilder.AppendLine("First matrix cast to double[,]:");
        
        for (int rowIndex = 0; rowIndex < firstMatrix.Size; ++rowIndex) {
          castingBuilder.Append("[ ");
          for (int colIndex = 0; colIndex < firstMatrix.Size; ++colIndex) {
            castingBuilder.Append($"{firstArray[rowIndex, colIndex],8:F2} ");
          }
          castingBuilder.AppendLine("]");
        }
        
        Console.Write(castingBuilder.ToString());

        // Demo of prototype (deep copy)
        string prototypeHeader = "\n--- Prototype Pattern (Deep Copy) ---";
        Console.WriteLine(prototypeHeader);
        
        SquareMatrix clonedMatrix = firstMatrix.DeepCopy();
        string clonedOutput = "Cloned Matrix:\n" + clonedMatrix.ToString();
        Console.Write(clonedOutput);
        
        // Modification of original doesn't affect copy
        firstMatrix[0, 0] = 999.99;
        
        string originalModifiedOutput = "Original after modification:\n" + firstMatrix.ToString();
        string cloneUnchangedOutput = "Clone remains unchanged:\n" + clonedMatrix.ToString();
        Console.Write(originalModifiedOutput + cloneUnchangedOutput);

        // Demo of Equals and GetHashCode
        string equalsHeader = "\n--- Equals and GetHashCode ---";
        Console.WriteLine(equalsHeader);
        
        SquareMatrix thirdMatrix = new SquareMatrix(3, -10, 10);
        
        string equalsOutput = $"First equals Third: {firstMatrix.Equals(thirdMatrix)}\n" +
                            $"First hash code: {firstMatrix.GetHashCode()}\n" +
                            $"Third hash code: {thirdMatrix.GetHashCode()}";
        Console.WriteLine(equalsOutput);

        // Demo of exception handling
        string exceptionHeader = "\n--- Exception Handling ---";
        Console.WriteLine(exceptionHeader);
        
        try {
          string invalidSizeMessage = "Attempting to create matrix with invalid size...";
          Console.WriteLine(invalidSizeMessage);
          SquareMatrix invalidMatrix = new SquareMatrix(0);
        } catch (ArgumentException exception) {
          string caughtMessage = $"Caught exception: {exception.Message}";
          Console.WriteLine(caughtMessage);
        }

        try {
          string differentSizesMessage = "\nAttempting to add matrices of different sizes...";
          Console.WriteLine(differentSizesMessage);
          
          SquareMatrix smallMatrix = new SquareMatrix(2, 1, 5);
          SquareMatrix resultMatrix = firstMatrix + smallMatrix;
        } catch (MatrixDimensionException exception) {
          string caughtMessage = $"Caught exception: {exception.Message}";
          Console.WriteLine(caughtMessage);
        }

        try {
          string invalidIndexMessage = "\nAttempting to access invalid index...";
          Console.WriteLine(invalidIndexMessage);
          
          double value = firstMatrix[10, 10];
        } catch (IndexOutOfRangeException exception) {
          string caughtMessage = $"Caught exception: {exception.Message}";
          Console.WriteLine(caughtMessage);
        }

      } catch (Exception exception) {
        string unexpectedErrorMessage = $"Unexpected error: {exception.Message}";
        Console.WriteLine(unexpectedErrorMessage);
      }

      string footerMessage = "\n==========================================\n" +
                           "     DEMO COMPLETED\n" +
                           "==========================================";
      Console.WriteLine(footerMessage);
    }

  }

}