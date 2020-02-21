using System;                                               //Тут подключаются библиотеки
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Frolov_calcul
{ 
    public partial class Form1 : Form
    {
        StreamWriter dosave = File.CreateText(@"c:\save\file.txt");
        bool operacia_perenesena_uspeshno, oshibka;                 //Объявление переменных логического типа
        double resultat, first, sec;                                //Объявление переменных с плавующей точкой, которые хранят результат, первое число и второе число
        int place;                                                  //Место, в котором стоит оператор
        char sho_delt;                                              //Переменная хранит действие, которое хочет использовать пользователь
        char[] dei = new char[3] { 'm', 'l', '√' };                 //Массив хранит всевозможные действия, выполнение которых отличается от большенства
        public Form1()
        {
            InitializeComponent();

        }

        private void chislo(object sender, EventArgs e)             //Функция обрабатывающая нажатие числа
        {
            textBox1.Text += ((Button)sender).Text;
            dosave.Write(((Button)sender).Text);
        }

        private void matematichka(object sender, EventArgs e)               //Функция обрабатывающая нажатия на кнопки некоторых (+, -, /, *, %, ^) математических действий 
        {
            if (!operacia_perenesena_uspeshno && textBox1.TextLength>0) //Выполняется, если оператор ещё не был нажат, а длина строки имеет хотя бы одно число.
            {
                sho_delt = ((Button)sender).Text[0];
                dosave.Write(((Button)sender).Text[0]);
                textBox1.Text += sho_delt;
                operacia_perenesena_uspeshno = true;
                place = textBox1.TextLength;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            dosave.Close();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (operacia_perenesena_uspeshno && place < textBox1.TextLength)                                    //Функция, обрабатывающая нажатие кнопки считать
            {
                if (! dei.Contains(sho_delt)) first = Convert.ToDouble(textBox1.Text.Substring(0, place - 1));  //Заполнение переменных относительно символа математической операции
                sec = Convert.ToDouble(textBox1.Text.Substring(place, textBox1.Text.Length - place));
                switch (sho_delt)                                                                                //выполнение операций, в зависимости от выбранной 
                {
                    case '+':
                        resultat = first + sec;
                        break;
                    case '-':
                        resultat = first - sec;
                        break;
                    case '*':
                        resultat = first * sec;
                        break;
                    case '/':
                        if (sec == 0) oshibka = true;                                                           //При делении на ноль переменной oshibka присваивается значение true
                        else resultat = first / sec;
                        break;
                    case '%':
                        resultat = first * sec / 100;
                        break;
                    case '^':
                        resultat = System.Math.Pow(first, sec);
                        break;
                    case 'm':
                        resultat = System.Math.Abs(sec);
                        textBox1.Text += ")";
                        break;
                    case 'l':
                        if (sec == 0) oshibka = true;                                                           //По определению нельзя найти логарифм нулевого числа.
                        else
                        {
                            resultat = System.Math.Log(sec);
                            textBox1.Text += ")";
                        }
                        break;
                    case '√':
                        if (sec < 0) oshibka = true;
                        else resultat = System.Math.Sqrt(sec);
                        break;

                }
                operacia_perenesena_uspeshno = false;
                if (oshibka)                                                                                    //Если во время вычислений произошла ошибка, то выводим на экран надпись "Ошибка!"
                {
                    label1.Text = "Ошибка!";
                    textBox1.Text = "";
                }
                else                                                                                            //Если ошибок замемчено не было, то выводим на экран решение примера, а в текстовом поле оставляем результат преобразования
                {
                    label1.Text = textBox1.Text + " = " + Convert.ToString(resultat);
                    dosave.Write(" = " + Convert.ToString(resultat));
                    textBox1.Text = Convert.ToString(resultat);
                }
                oshibka = false;
                place = 0;
            }
        }

    private void mat_2(object sender, EventArgs e)  //Функция обрабатывает нажатия клавишь логарифма, модуля и корня. Выенесена отдельно, т.к. отличается от других математических операций тем, что может работать с одним числом
        {
            sho_delt = ((Button)sender).Text[0];
            switch (sho_delt)
            {
                case 'l':
                    textBox1.Text = "ln(";
                    dosave.Flush();
                    dosave.Write("ln(");
                    break;
                case 'm':
                    textBox1.Text = "mod(";
                    dosave.Flush();
                    dosave.Write("mod(");
                    break;
                default:
                    textBox1.Text = "√";
                    dosave.Flush();
                    dosave.Write("√");
                    break;
            }
            operacia_perenesena_uspeshno = true;
            place = textBox1.TextLength;
        }

        private void button21_Click(object sender, EventArgs e)         //Функция позволяет стереть содержимое текстового поля, а так же привести все перменные в исходное состояние
        {
            textBox1.Text = "";
            place = 0;
            oshibka = false;
            operacia_perenesena_uspeshno = false;
            dosave.WriteLine("");
        }
    }
}