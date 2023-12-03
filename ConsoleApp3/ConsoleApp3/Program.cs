using System;

public class Quaternion
{
    public double W { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public Quaternion(double w, double x, double y, double z)
    {
        W = w;
        X = x;
        Y = y;
        Z = z;
    }

    // Overloaded addition operator for quaternions
    public static Quaternion operator +(Quaternion q1, Quaternion q2)
    {
        return new Quaternion(q1.W + q2.W, q1.X + q2.X, q1.Y + q2.Y, q1.Z + q2.Z);
    }

    // Overloaded subtraction operator for quaternions
    public static Quaternion operator -(Quaternion q1, Quaternion q2)
    {
        return new Quaternion(q1.W - q2.W, q1.X - q2.X, q1.Y - q2.Y, q1.Z - q2.Z);
    }

    // Overloaded multiplication operator for quaternions
    public static Quaternion operator *(Quaternion q1, Quaternion q2)
    {
        double w = q1.W * q2.W - q1.X * q2.X - q1.Y * q2.Y - q1.Z * q2.Z;
        double x = q1.W * q2.X + q1.X * q2.W + q1.Y * q2.Z - q1.Z * q2.Y;
        double y = q1.W * q2.Y - q1.X * q2.Z + q1.Y * q2.W + q1.Z * q2.X;
        double z = q1.W * q2.Z + q1.X * q2.Y - q1.Y * q2.X + q1.Z * q2.W;

        return new Quaternion(w, x, y, z);
    }

    // Method to calculate the norm of a quaternion
    public double Norm()
    {
        return Math.Sqrt(W * W + X * X + Y * Y + Z * Z);
    }

    // Method to get the conjugate of a quaternion
    public Quaternion Conjugate()
    {
        return new Quaternion(W, -X, -Y, -Z);
    }

    // Method to get the inverse of a quaternion
    public Quaternion Inverse()
    {
        double normSquared = W * W + X * X + Y * Y + Z * Z;
        if (normSquared == 0)
        {
            throw new InvalidOperationException("Cannot invert a quaternion with zero norm.");
        }

        double invNormSquared = 1.0 / normSquared;
        return new Quaternion(W * invNormSquared, -X * invNormSquared, -Y * invNormSquared, -Z * invNormSquared);
    }

    // Overloaded equality operator for quaternions
    public static bool operator ==(Quaternion q1, Quaternion q2)
    {
        return q1.W == q2.W && q1.X == q2.X && q1.Y == q2.Y && q1.Z == q2.Z;
    }

    // Overloaded inequality operator for quaternions
    public static bool operator !=(Quaternion q1, Quaternion q2)
    {
        return !(q1 == q2);
    }

    // Conversion from quaternion to 3x3 rotation matrix
    public double[,] ToRotationMatrix()
    {
        double[,] matrix = new double[3, 3];

        matrix[0, 0] = 1 - 2 * (Y * Y + Z * Z);
        matrix[0, 1] = 2 * (X * Y - W * Z);
        matrix[0, 2] = 2 * (X * Z + W * Y);

        matrix[1, 0] = 2 * (X * Y + W * Z);
        matrix[1, 1] = 1 - 2 * (X * X + Z * Z);
        matrix[1, 2] = 2 * (Y * Z - W * X);

        matrix[2, 0] = 2 * (X * Z - W * Y);
        matrix[2, 1] = 2 * (Y * Z + W * X);
        matrix[2, 2] = 1 - 2 * (X * X + Y * Y);

        return matrix;
    }

    // Conversion from 3x3 rotation matrix to quaternion
    public static Quaternion FromRotationMatrix(double[,] matrix)
    {
        double trace = matrix[0, 0] + matrix[1, 1] + matrix[2, 2];

        if (trace > 0)
        {
            double s = 0.5 / Math.Sqrt(trace + 1.0);
            double w = 0.25 / s;
            double x = (matrix[2, 1] - matrix[1, 2]) * s;
            double y = (matrix[0, 2] - matrix[2, 0]) * s;
            double z = (matrix[1, 0] - matrix[0, 1]) * s;

            return new Quaternion(w, x, y, z);
        }
        else if (matrix[0, 0] > matrix[1, 1] && matrix[0, 0] > matrix[2, 2])
        {
            double s = 2.0 * Math.Sqrt(1.0 + matrix[0, 0] - matrix[1, 1] - matrix[2, 2]);
            double w = (matrix[2, 1] - matrix[1, 2]) / s;
            double x = 0.25 * s;
            double y = (matrix[0, 1] + matrix[1, 0]) / s;
            double z = (matrix[0, 2] + matrix[2, 0]) / s;

            return new Quaternion(w, x, y, z);
        }
        else if (matrix[1, 1] > matrix[2, 2])
        {
            double s = 2.0 * Math.Sqrt(1.0 + matrix[1, 1] - matrix[0, 0] - matrix[2, 2]);
            double w = (matrix[0, 2] - matrix[2, 0]) / s;
            double x = (matrix[0, 1] + matrix[1, 0]) / s;
            double y = 0.25 * s;
            double z = (matrix[1, 2] + matrix[2, 1]) / s;

            return new Quaternion(w, x, y, z);
        }
        else
        {
            double s = 2.0 * Math.Sqrt(1.0 + matrix[2, 2] - matrix[0, 0] - matrix[1, 1]);
            double w = (matrix[1, 0] - matrix[0, 1]) / s;
            double x = (matrix[0, 2] + matrix[2, 0]) / s;
            double y = (matrix[1, 2] + matrix[2, 1]) / s;
            double z = 0.25 * s;

            return new Quaternion(w, x, y, z);
        }
    }

    public override string ToString()
    {
        return $"Quaternion(W: {W}, X: {X}, Y: {Y}, Z: {Z})";
    }

    public override bool Equals(object obj)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (ReferenceEquals(obj, null))
        {
            return false;
        }

        throw new NotImplementedException();
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}

class Program
{
    static void Main()
    {
        // Example usage:

        // Creating quaternions
        Quaternion q1 = new Quaternion(1, 2, 3, 4);
        Quaternion q2 = new Quaternion(5, 6, 7, 8);

        // Arithmetic operations
        Quaternion sum = q1 + q2;
        Quaternion difference = q1 - q2;
        Quaternion product = q1 * q2;

        Console.WriteLine($"Sum: {sum}");
        Console.WriteLine($"Difference: {difference}");
        Console.WriteLine($"Product: {product}");

        // Norm, conjugate, and inverse
        Console.WriteLine($"Norm of q1: {q1.Norm()}");
        Console.WriteLine($"Conjugate of q1: {q1.Conjugate()}");
        Console.WriteLine($"Inverse of q1: {q1.Inverse()}");

        // Equality and inequality
        Console.WriteLine($"Are q1 and q2 equal? {q1 == q2}");
        Console.WriteLine($"Are q1 and q2 not equal? {q1 != q2}");

        // Conversion to rotation matrix
        double[,] rotationMatrix = q1.ToRotationMatrix();
        Console.WriteLine("Rotation Matrix:");
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Console.Write(rotationMatrix[i, j] + " ");
            }
            Console.WriteLine();
        }

        // Conversion from rotation matrix
        Quaternion qFromMatrix = Quaternion.FromRotationMatrix(rotationMatrix);
        Console.WriteLine($"Quaternion from Rotation Matrix: {qFromMatrix}");
    }
}
