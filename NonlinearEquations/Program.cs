using System;

namespace NonlinearEquations
{
    class Program
    {
        //Лабораторная работа №1
        //Глухова В. ИС-51
        //Вариант 9: cos(x) - x = 0
        static double F(double x) => Math.Cos(x) - x; //Функция f(x)
        static double DF(double x) => -Math.Sin(x) - 1; //F'(x) для метода Ньютона
        static double Phi(double x) => Math.Cos(x); //Фи для метода простой итерации

        //1. Метод бисекции
        static (double root, int iterations, double residual) Bisection(double a, double b, double eps) //
        {
            Console.WriteLine($"{"n",-4} {"a",-12} {"b",-12} {"c",-12} {"f(c)",-14} {"b-a",-12}"); //для таблицы (стр.16-17)
            Console.WriteLine(new string('-', 72));

            int n = 0;
            while ((b - a) > eps)
            {
                double c = (a + b) / 2.0;
                Console.WriteLine($"{n,-4} {a,-12:F6} {b,-12:F6} {c,-12:F6} {F(c),-14:F6} {b - a,-12:F6}");
                if (F(a) * F(c) < 0)
                    b = c;
                else
                    a = c;
                n++;
            }
            double root = (a + b) / 2.0;
            return (root, n, Math.Abs(F(root)));
        }

        //2. Метод простой итерации
        static (double root, int iterations, double residual) SimpleIteration(double x0, double eps, int maxIter = 1000)
        {
            Console.WriteLine($"{"n",-4} {"x_n",-12} {"x_(n+1)",-12} {"|dx|",-12} {"f(x_(n+1))",-12}");
            Console.WriteLine(new string('-', 60));

            double xPrev = x0;
            for (int n = 1; n <= maxIter; n++)
            {
                double xNext = Phi(xPrev);
                Console.WriteLine($"{n,-4} {xPrev,-12:F6} {xNext,-12:F6} {Math.Abs(xNext - xPrev),-12:F6} {F(xNext),-12:F6}");

                if (Math.Abs(xNext - xPrev) < eps)
                    return (xNext, n, Math.Abs(F(xNext)));

                xPrev = xNext;
            }
            return (xPrev, maxIter, Math.Abs(F(xPrev)));
        }

        //3. Метод Ньютона
        static (double root, int iterations, double residual) Newton(double x0, double eps, int maxIter = 1000)
        {
            Console.WriteLine($"{"k",-4} {"x_k",-12} {"f(x_k)",-14} {"f'(x_k)",-14} {"x_(k+1)",-12} {"|dx|",-12}");
            Console.WriteLine(new string('-', 76));

            double x = x0;
            for (int n = 1; n <= maxIter; n++)
            {
                if (Math.Abs(DF(x)) < 1e-12)
                {
                    Console.WriteLine("Ошибка: производная близка к нулю.");
                    break;
                }
                double xNext = x - F(x) / DF(x);
                Console.WriteLine($"{n,-4} {x,-12:F6} {F(x),-14:F6} {DF(x),-14:F6} {xNext,-12:F6} {Math.Abs(xNext - x),-12:F6}");

                if (Math.Abs(xNext - x) < eps)
                    return (xNext, n, Math.Abs(F(xNext)));

                x = xNext;
            }
            return (x, maxIter, Math.Abs(F(x)));
        }

        //4. Метод секущих
        static (double root, int iterations, double residual) Secant(double x0, double x1, double eps, int maxIter = 1000)
        {
            Console.WriteLine($"{"k",-4} {"x_k",-12} {"f(x_k)",-14} {"x_(k+1)",-12} {"|dx|",-12}");
            Console.WriteLine(new string('-', 60));

            for (int n = 1; n <= maxIter; n++)
            {
                double denom = F(x1) - F(x0);
                if (Math.Abs(denom) < 1e-12)
                {
                    Console.WriteLine("Ошибка: деление на ноль.");
                    break;
                }
                double xNext = x1 - F(x1) * (x1 - x0) / denom;
                Console.WriteLine($"{n,-4} {x1,-12:F6} {F(x1),-14:F6} {xNext,-12:F6} {Math.Abs(xNext - x1),-12:F6}");

                if (Math.Abs(xNext - x1) < eps)
                    return (xNext, n, Math.Abs(F(xNext)));

                x0 = x1;
                x1 = xNext;
            }
            return (x1, maxIter, Math.Abs(F(x1)));
        }
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            double eps = 0.0001;
            double a = 0.0, b = 1.0;        // отрезок [0, 1]
            double x0Iter = 0.5;            // для простой итерации (из таблицы Б.1)
            double x0Newton = 1.0;          // для Ньютона: f(1)·f''(1) > 0

            Console.WriteLine("=== ВАРИАНТ 9: cos(x) - x = 0 ===");
            Console.WriteLine($"Отрезок: [{a}; {b}], точность ε = {eps}\n");

            // 1. Бисекция
            Console.WriteLine("--- Метод бисекции ---");
            var r1 = Bisection(a, b, eps);
            Console.WriteLine($"Ответ: x = {r1.root:F6}, итераций = {r1.iterations}, невязка = {r1.residual:E2}\n");

            // 2. Простая итерация
            Console.WriteLine("--- Метод простой итерации ---");
            var r2 = SimpleIteration(x0Iter, eps);
            Console.WriteLine($"Ответ: x = {r2.root:F6}, итераций = {r2.iterations}, невязка = {r2.residual:E2}\n");

            // 3. Ньютон
            Console.WriteLine("--- Метод Ньютона ---");
            var r3 = Newton(x0Newton, eps);
            Console.WriteLine($"Ответ: x = {r3.root:F6}, итераций = {r3.iterations}, невязка = {r3.residual:E2}\n");

            // 4. Секущих
            Console.WriteLine("--- Метод секущих ---");
            var r4 = Secant(a, b, eps);
            Console.WriteLine($"Ответ: x = {r4.root:F6}, итераций = {r4.iterations}, невязка = {r4.residual:E2}\n");

            // Сравнение
            Console.WriteLine("=== СРАВНЕНИЕ МЕТОДОВ ===");
            Console.WriteLine($"{"Метод",-22} {"Корень",-12} {"Итераций",-12} {"Невязка",-12}");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"{"Бисекция",-22} {r1.root,-12:F6} {r1.iterations,-12} {r1.residual,-12:E2}");
            Console.WriteLine($"{"Простая итерация",-22} {r2.root,-12:F6} {r2.iterations,-12} {r2.residual,-12:E2}");
            Console.WriteLine($"{"Ньютон",-22} {r3.root,-12:F6} {r3.iterations,-12} {r3.residual,-12:E2}");
            Console.WriteLine($"{"Секущих",-22} {r4.root,-12:F6} {r4.iterations,-12} {r4.residual,-12:E2}");

            Console.WriteLine("\nГотово. Нажмите любую клавишу для выхода.");
            Console.ReadKey();
        }
    }
}