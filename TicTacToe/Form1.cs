using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tictactoe
{
    public partial class Form1 : Form
    {
        bool turn = true; // true - ход игрока Х, false - ход игрока Y
        bool pc_comp = false; // ход компьютера (ПК) - соответственно, базово стоит false, т.к. это - противопоставляемый игрок
        int turn_count = 0; // переменная, отвечающая за ходы в игре
        static string player1, player2;
        
        public Form1()
        {
            InitializeComponent();
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {           
            player1 = Convert.ToString(textBox1.Text);       // имя Игрока Х (введённое им)
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            player2 = Convert.ToString(textBox2.Text);     // имя Игрока Х (введённое им)            
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)   // начало игры
        {
            gameStart();
        }
              
        private void gameStart()    // начало игры
        {
            turn = true;        // ход игрока
            turn_count = 0;     // начало "отсчета" ходов
            foreach (Control c in Controls)
            {
                try
                {

                    Button b = (Button)c;
                    b.Enabled = true;
                    b.Text = "";

                }
                catch { }
            }
        }
        
        private void resultClear()    // сброс результатов игры - в этом случае, соответственно, в каждое поле присваивается значение "0" (см. ниже)
        {
            X_win.Text = "0";      
            Y_win.Text = "0";
            draw.Text = "0";
        }
        
        private void АвторToolStripMenuItem_Click_1(object sender, EventArgs e)     // предоставление информации об авторе
        {
            MessageBox.Show("Рудой Е.М., Прог-С-18");     // окно с информацией
        }
        
        private void выходToolStripMenuItem_Click_1(object sender, EventArgs e)     // выход из игры (с появлением окна окна, уверен ли пользователь в этом)
        {
            if(MessageBox.Show("Вы точно хотите выйти?", "Крестики-нолики", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK) this.Close();           
        }

        private void button_click(object sender, EventArgs e)
        {
            {
                Button b = (Button)sender;
                if (turn)
                    b.Text = "X"; // отображение "Х" в качестве хода игрока Х (т.е. 1-й игрок)

                else
                    b.Text = "O"; // отображение "0" в качестве хода игрока У (или ПК, соответственно)

                turn = !turn;  
                b.Enabled = false;
                turn_count++;

                checkWinner();   // вызов метода для определения победителя (см. ниже в коде)
            }
            if ((!turn) && (pc_comp))     // если 2-й игрок - ПК, и нами сделан ход, вызываем метод для ходов ПК
            {
                computer_make_move();
            }

        }

        private void computer_make_move()   // метод для ходов ПК (ниже прописана их общая логика)
        {

            Button move = null;   // до ходов компьютера, т.е. null


            move = look_for_win_or_block("O");         // поиск, куда поставить ход для выигрыша (чтобы замкнуть комбинацию) 
            if (move == null)                          // если таких ходов нет, вызываем метод для блокирования хода игрока-противника, чтобы не дать ему замкнуть комбинацию
            {
                move = look_for_win_or_block("X");     
                if (move == null)                      // если ходов выше нет, ищем угол, куда ходить (чтобы хоть как-то увеличить вероятность благоприятного исхода)
                {
                    move = look_for_corner();          
                    if (move == null)                  // если и таких ходов нет, ищем просто свободное пространство для хода 
                    {
                        move = look_for_open_space();
                    }
                }
            }

            if (turn_count != 9)                        // если не набралось 9 ходов (не заполнилось всё поле), то ходы продолжаются
                move.PerformClick();
        }
        
        private Button look_for_open_space()                // расписываем метод для поиска компьютером свободного пространства для хода
        {
            Console.WriteLine("Looking for open space");    
            Button b = null;                        // базовая кнопка для занесения в нее хода
            foreach (Control c in Controls)
            {
                b = c as Button;                    // в кнопку "b" заносим ход кнопки "с"
                if (b != null)                      // если ход возможен, он выполняется, и этот результат возвращаем в главный метод (выше в коде)
                {
                    if (b.Text == "")
                        return b;
                }
            }
            return null;
        }                       

        
        private Button look_for_corner()               // метод для поиска угла для хода (с целью увеличения вероятности благоприятного исхода)
        {
            Console.WriteLine("Looking for corner"); 
            if (A1.Text == "O")         // если уже был ход в клетку А1, то логично, что нужно ходить в противоположные углы С1, А3 или С3 (чтобы потом, в идеале, замкнуть клетку между А1 и                           
            {                           // каким-то их этих 3-х углов (для выигрыша)
                if (A3.Text == "")      
                    return A3;
                if (C3.Text == "")
                    return C3;
                if (C1.Text == "")
                    return C1;
            }

            if (A3.Text == "O")        // логика та же, что и выше
            {
                if (A1.Text == "")
                    return A1;
                if (C3.Text == "")
                    return C3;
                if (C1.Text == "")
                    return C1;
            }

            if (C3.Text == "O")        // логика та же, что и выше
            {
                if (A1.Text == "")
                    return A1;
                if (A3.Text == "")
                    return A3;
                if (C1.Text == "")
                    return C1;
            }

            if (C1.Text == "O")         // логика та же, что и выше
            {
                if (A1.Text == "")
                    return A1;
                if (A3.Text == "")
                    return A3;
                if (C3.Text == "")
                    return C3;
            }

            if (A1.Text == "")         // аналогично пробегаемся по углам диагоналей, в целом
                return A1;
            if (A3.Text == "")
                return A3;
            if (C1.Text == "")
                return C1;
            if (C3.Text == "")
                return C3;

            return null;
        }
        
        private Button look_for_win_or_block(string mark)            // метод для поиска ходов для победы
        {
            
            // проверяем по горизонтали

            if ((A1.Text == mark) && (A2.Text == mark) && (A3.Text == ""))    // если ходили в А1 и А2, то логично, что требуется ходить в А3. Возвращаем А3 в главный метод,
                return A3;                                                    // соответственно, для хода в эту клетку. Ниже по коду логика та же самая.
            if ((A2.Text == mark) && (A3.Text == mark) && (A1.Text == ""))
                return A1;
            if ((A1.Text == mark) && (A3.Text == mark) && (A2.Text == ""))
                return A2;

            if ((B1.Text == mark) && (B2.Text == mark) && (B3.Text == ""))
                return B3;
            if ((B2.Text == mark) && (B3.Text == mark) && (B1.Text == ""))
                return B1;
            if ((B1.Text == mark) && (B3.Text == mark) && (B2.Text == ""))
                return B2;

            if ((C1.Text == mark) && (C2.Text == mark) && (C3.Text == ""))
                return C3;
            if ((C2.Text == mark) && (C3.Text == mark) && (C1.Text == ""))
                return C1;
            if ((C1.Text == mark) && (C3.Text == mark) && (C2.Text == ""))
                return C2;

            // проверяем по вертикали

            if ((A1.Text == mark) && (B1.Text == mark) && (C1.Text == ""))
                return C1;
            if ((B1.Text == mark) && (C1.Text == mark) && (A1.Text == ""))
                return A1;
            if ((A1.Text == mark) && (C1.Text == mark) && (B1.Text == ""))
                return B1;

            if ((A2.Text == mark) && (B2.Text == mark) && (C2.Text == ""))
                return C2;
            if ((B2.Text == mark) && (C2.Text == mark) && (A2.Text == ""))
                return A2;
            if ((A2.Text == mark) && (C2.Text == mark) && (B2.Text == ""))
                return B2;

            if ((A3.Text == mark) && (B3.Text == mark) && (C3.Text == ""))
                return C3;
            if ((B3.Text == mark) && (C3.Text == mark) && (A3.Text == ""))
                return A3;
            if ((A3.Text == mark) && (C3.Text == mark) && (B3.Text == ""))
                return B3;

            // проверяем по диагонали

            if ((A1.Text == mark) && (B2.Text == mark) && (C3.Text == ""))
                return C3;
            if ((B2.Text == mark) && (C3.Text == mark) && (A1.Text == ""))
                return A1;
            if ((A1.Text == mark) && (C3.Text == mark) && (B2.Text == ""))
                return B2;

            if ((A3.Text == mark) && (B2.Text == mark) && (C1.Text == ""))
                return C1;
            if ((B2.Text == mark) && (C1.Text == mark) && (A3.Text == ""))
                return A3;
            if ((A3.Text == mark) && (C1.Text == mark) && (B2.Text == ""))
                return B2;

            return null;
        }
        // Метод определяющий победителя, выводящий окно с именем победителя и увиличивающий счетчик побед и ничьих
        private void checkWinner()        // метод для определения победителя
        {
            bool is_a_winner = false;     // базовое значение переменной победителя - false
            if ((A1.Text == A2.Text) && (A2.Text == A3.Text) && (!A1.Enabled))       // но если замкнулись соответствующие клетки для победы, в эту переменную присваиваем значение - true
                is_a_winner = true;
            else if ((B1.Text == B2.Text) && (B2.Text == B3.Text) && (!B1.Enabled))  // далее по коду логика та же самая
                is_a_winner = true;
            else if ((C1.Text == C2.Text) && (C2.Text == C3.Text) && (!C1.Enabled))
                is_a_winner = true;
            else if ((A1.Text == B1.Text) && (B1.Text == C1.Text) && (!A1.Enabled))
                is_a_winner = true;
            else if ((A2.Text == B2.Text) && (B2.Text == C2.Text) && (!A2.Enabled))
                is_a_winner = true;
            else if ((A3.Text == B3.Text) && (B3.Text == C3.Text) && (!A3.Enabled))
                is_a_winner = true;
            else if ((A1.Text == B2.Text) && (B2.Text == C3.Text) && (!A1.Enabled))
                is_a_winner = true;
            else if ((A3.Text == B2.Text) && (B2.Text == C1.Text) && (!C1.Enabled))
                is_a_winner = true;


            if (is_a_winner)
            {
                disabledButtons();     // при выводе победителя клетки для ходов больше не активны (за ненадобностью)
                string winner = " ";
                if (turn)                 // если выигрывает Игрок Y (2-й игрок), выводим соответствующее сообщение об этом
                {
                    winner = player2;
                    Y_win.Text = (Int32.Parse(Y_win.Text) + 1).ToString();  // заносим это в счетчик Игрока У               

                }
                else                      // если выигрывает Игрок Х (1-й игрок), выводим соответствующее сообщение об этом
                {
                    winner = player1;      
                    X_win.Text = (Int32.Parse(X_win.Text) + 1).ToString();    // заносим это в счетчик Игрока Х                         
                }

                MessageBox.Show("Победа " + winner , "Итог");     // непосредственно текст сообщения

            }
            else
            {
                if (turn_count == 9)    // если же все клетки "забиты", и нет победителя, то происходит ничья с соответствующим сообщением
                {
                    draw.Text = (Int32.Parse(draw.Text) + 1).ToString();   // заносим это в счетчик ничьих
                    MessageBox.Show("У вас ничья!", "Ничья");
                }
            }


        }
        
        private void disabledButtons()      // метод для блокировки клеток после игры
        {
            
                foreach (Control c in Controls)
                {
                try
                    {
                    Button b = (Button)c;
                    b.Enabled = false;
                   
                    }
                catch { }
            }
            
        }

        
        private void button_enter(object sender, EventArgs e)   // метод для отображения "О" или "Х" при наведении на клетку
        {
            Button b = (Button)sender;
            if (b.Enabled)
            {
                if (turn)             // при первональном ходе отображается "Х"
                    b.Text = "X";
                else                  // при следующем ходе отображается "О"
                    b.Text = "O";
            }
        }
       
        private void button_leave(object sender, EventArgs e)   // если убрали курсор, то ничего не оторажается
        {
            Button b = (Button)sender;
            if (b.Enabled)
            {
                b.Text = "";               
            }
        }
        
        private void ГлавноеМенюToolStripMenuItem_Click(object sender, EventArgs e)   // метод для выхода в Form2 (главное меню)
        {
            Form1 f1 = new Form1();
            this.Visible = false;
            Form2 f2 = new Form2();
            f2.ShowDialog();
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "Игрок Х";                     // назначить имя Игроку 1 при стандартном запуске
            textBox2.Text = "Игрок Y";                     // назначить имя Игроку 2 при стандартном запуске
        }
       
        private void ПравилаToolStripMenuItem_Click(object sender, EventArgs e)   // показать правила игры
        {
            MessageBox.Show("Игроки по очереди ставят на свободные клетки поля 3х3 знаки (один всегда крестики, другой всегда нолики). Первый, выстроивший в ряд 3 своих фигур по вертикали, горизонтали или диагонали, выигрывает. Первый ход делает игрок, ставящий крестики.");
        }                  
        
        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)   // включить компьютер в качестве противника
        {

            if (checkBox1.Checked == true)         // если галочка стоить в соответтсвующем чекбоксе, запускаем игру и меняем имя Игрока 2 на "Компьютер"
            {
                gameStart();
                resultClear();
                pc_comp = true;
                textBox1.Text = "Игрок Х";
                textBox2.Text = "Компьютер";
            }
            else                  // в противном случае - просто запускаем игру (без изменения имени)
            {
                gameStart();
                resultClear();
                pc_comp = false;
                textBox1.Text = "Игрок Х";
                textBox2.Text = "Игрок Y";
                textBox2.ReadOnly = false;
            }
        }
        
        private void СброситьРезультатыToolStripMenuItem_Click(object sender, EventArgs e)    // сбросить (обнулить) результаты всех игр
        {
            resultClear();           
        }
    }
}