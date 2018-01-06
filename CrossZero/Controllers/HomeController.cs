using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Threading.Tasks;
using CrossZero.Models;
using System.Data.Entity;
namespace CrossZero.Controllers
{
    public class HomeController : Controller
    {
        private GameBaseContext db = new GameBaseContext();


        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public string calc(string history)                         // надо сделать Асинхронность !!!!!!!!!!!!!!!!!!!!!!!!!
        {
            int[,] ugol = new int[4, 2] { { 0, 0 }, { 0, 2 }, { 2, 2 }, { 2, 0 } };// Угловые ячейки
            int[,] bock = new int[4, 2] { { 0, 1 }, { 1, 2 }, { 2, 1 }, { 1, 0 } };// Боковые ячейки
            int[][,] diag = new int[8][,] ;// Диагонали
            diag[0] = new int[3, 2] { { 0, 0 }, { 0, 1 }, { 0, 2 } };
            diag[1] = new int[3, 2] { { 1, 0 }, { 1, 1 }, { 1, 2 } };
            diag[2] = new int[3, 2] { { 2, 0 }, { 2, 1 }, { 2, 2 } };

            diag[3] = new int[3, 2] { { 0, 0 }, { 1, 0 }, { 2, 0 } };
            diag[4] = new int[3, 2] { { 0, 1 }, { 1, 1 }, { 2, 1 } };
            diag[5] = new int[3, 2] { { 0, 2 }, { 1, 2 }, { 2, 2 } };

            diag[6] = new int[3, 2] { { 0, 0 }, { 1, 1 }, { 2, 2 } };
            diag[7] = new int[3, 2] { { 0, 2 }, { 1, 1 }, { 2, 0 } };


            int[,] mas = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
            int count = 0;
            List<int[]> lstHod = new List<int[]>();

            if (history != "")
            {
                string[] masstrhod = history.Split('|');
                count = masstrhod.Length;               // Количество сделанных ходов                
                for (int i = 0; i < masstrhod.Length; i++)
                {
                    string[] mashod = masstrhod[i].Split('/');
                    //mas[Convert.ToInt32(mashod[1]), Convert.ToInt32(mashod[2])] = Convert.ToInt32(mashod[0]);// ставить надо 1 или 4 для сравнений
                    lstHod.Add(new int[2]{ Convert.ToInt32(mashod[1]),Convert.ToInt32(mashod[2])});
                    mas[Convert.ToInt32(mashod[1]), Convert.ToInt32(mashod[2])] = (i % 2) + 1;// ставить надо 1 -крестик или 2-нолик для сравнений

                }
            }

            Random rnd = new Random();

            int simvol = (count % 2) + 1; // 1 -крестик или 2-нолик символ этого хода
            if (simvol == 1)                                        // Не обязательно анализироать на символ
            {                         //Выбр хода для крестиков
                if (count == 0)
                {  //Если ходов небыло ходим в центр
                    return "1:1";
                }
                if (count == 2)   //Если ходов 2 те это третий ход выставляем крестик напротив выставленного нолика
                {
                    if (((lstHod[1][0] | lstHod[1][1]) & 1) == 1)  // нолик был выставлен в боковую ячейку
                    {
                        if ((lstHod[1][0] % 2) == 0)
                        {
                            return ((lstHod[1][0] + 2) % 4).ToString() + ":" + (rnd.Next(0, 2) * 2).ToString();
                        }
                        else
                        {
                            return (rnd.Next(0, 2) * 2).ToString() + ":" + ((lstHod[1][1] + 2) % 4).ToString();
                        }
                    }
                    else// нолик был выставлен в угловую ячейку 
                    {
                        return ((lstHod[1][0] + 2) % 4).ToString() + ":" + ((lstHod[1][1] + 2) % 4).ToString();
                    }
                }

                if (count == 4)
                {
                    for (int i = 0; i < 8; i++) // Проверка возможности выиграть этим ходом
                    {
                        int isinsert = DetectInDiag(1, ref diag[i], ref mas);
                        if (isinsert >= 0)
                        {
                            return diag[i][isinsert, 0].ToString() + ":" + diag[i][isinsert, 1].ToString();
                        }
                    }

                    for (int i = 0; i < 8; i++) // Проверяем необходимость вставить крестик между ноликами
                    {
                        int isinsert = DetectInDiag(2, ref diag[i], ref mas);
                        if (isinsert >= 0)
                        {
                            return diag[i][isinsert, 0].ToString() + ":" + diag[i][isinsert, 1].ToString();
                        }
                    }

                    // Ставим крестик в один из углов свободных  // Неправильно надо вставлять в угол против предидущего бокового нолика !!!
                    for (int i = 0; i < 4; i++)
                    {
                        if (mas[ugol[i, 0], ugol[i, 1]] == 0)
                        {
                            return ugol[i, 0].ToString() + ":" + ugol[i, 1].ToString();
                        }
                    }
                }

                if (count == 6)
                {
                    for (int i = 0; i < 8; i++) // Проверка возможности выиграть этим ходом
                    {
                        int isinsert = DetectInDiag(1, ref diag[i], ref mas);
                        if (isinsert >= 0)
                        {
                            return diag[i][isinsert, 0].ToString() + ":" + diag[i][isinsert, 1].ToString();
                        }
                    }

                    for (int i = 0; i < 8; i++) // Проверяем необходимость вставить крестик между ноликами где один свободный символ
                    {
                        int isinsert = DetectInDiag(2, ref diag[i], ref mas);
                        if (isinsert >= 0)
                        {
                            return diag[i][isinsert, 0].ToString() + ":" + diag[i][isinsert, 1].ToString();
                        }
                    }

                    // Проверка поставить символ в диагонали где два свободных символа
                    for (int i = 0; i < 8; i++) 
                    {
                        int isinsert = DetectInDiag(1, ref diag[i], ref mas, 1);
                        if (isinsert >= 0)
                        {
                            return diag[i][isinsert, 0].ToString() + ":" + diag[i][isinsert, 1].ToString();
                        }
                    }
                }

            }

            if (simvol == 2)  // Не обязательно анализироать на символ
            {
                if (count == 1)
                {  //Проверяем куда поставлен первый ход если был ход в центр ходим в любой угол если центр свободен занимаем центральную клетку
                    if ((lstHod[0][0] == 1) && (1 == lstHod[0][1]))
                    {
                        int nugol = rnd.Next(0, 4);
                        return ugol[nugol, 0].ToString() + ":" + ugol[nugol, 1].ToString();
                    }
                    else { return "1:1"; }
                }

                if (count == 3)
                {
                    for (int i = 0; i < 8; i++) // Проверяем необходимость вставить нолик между крестиками
                    {
                        int isinsert = DetectInDiag(1, ref diag[i], ref mas);
                        if (isinsert >= 0)
                        {
                            return diag[i][isinsert, 0].ToString() + ":" + diag[i][isinsert, 1].ToString();
                        }
                    }

                    if (mas[1, 1] == 1) // Если в центре стоит крестик то нолик в любой свободный угол
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (mas[ugol[i, 0], ugol[i, 1]] == 0)
                            {
                                return ugol[i, 0].ToString() + ":" + ugol[i, 1].ToString();
                            }
                        }
                    }
                    else
                    {
                        int isinsert = DetectInDiag(2, ref diag[1], ref mas, 1);
                        if (isinsert >= 0)
                        {
                            return diag[1][isinsert, 0].ToString() + ":" + diag[1][isinsert, 1].ToString();
                        }
                        isinsert = DetectInDiag(2, ref diag[4], ref mas, 1);
                        if (isinsert >= 0)
                        {
                            return diag[4][isinsert, 0].ToString() + ":" + diag[4][isinsert, 1].ToString();
                        }

                        return ((lstHod[0][0] & 2) | (lstHod[2][0] & 2)).ToString() + ":" + ((lstHod[0][1] & 2) | (lstHod[2][1] & 2)).ToString();
                    }

                }

                if ((count == 5) || (count == 7))
                {

                    for (int i = 0; i < 8; i++) // Проверка возможности выиграть этим ходом
                    {
                        int isinsert = DetectInDiag(2, ref diag[i], ref mas);
                        if (isinsert >= 0)
                        {
                            return diag[i][isinsert, 0].ToString() + ":" + diag[i][isinsert, 1].ToString();
                        }
                    }

                    for (int i = 0; i < 8; i++) // Проверяем необходимость вставить нолик  между крестами
                    {
                        int isinsert = DetectInDiag(1, ref diag[i], ref mas);
                        if (isinsert >= 0)
                        {
                            return diag[i][isinsert, 0].ToString() + ":" + diag[i][isinsert, 1].ToString();
                        }
                    }

                    // Проверка поставить символ в диагонали где два свободных символа
                    for (int i = 0; i < 8; i++)
                    {
                        int isinsert = DetectInDiag(2, ref diag[i], ref mas, 1);
                        if (isinsert >= 0)
                        {
                            return diag[i][isinsert, 0].ToString() + ":" + diag[i][isinsert, 1].ToString();
                        }
                    }
                }
            }


                // Проверка окончания игры
                //int end = TestEnd(ref mas, count);

                //if (end == 0)
                //{
                // Здесь нужно сделать алгоритм поиска правильного хода


                int intrnd = rnd.Next(0, 9 - count);  // Выбираем случайно порядковый номер свободной клетки [min max[
                intrnd++;
                int n = 0;                              // Счетчик для нахождения случайной ячейки
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (mas[row, col] == 0)
                    {
                        n++;
                        if (n == intrnd)
                        {
                            //count++;
                            //int simbol = 4;
                            //if ((count % 2) == 1) { simbol = 1; }
                            //mas[row, col] = simbol; // вычислять в зависимости от жномера хода крестики по не четным
                                                      //end = TestEnd(ref mas, count);

                            //return end.ToString() + ":" + row.ToString() + ":" + col.ToString();
                            return row.ToString() + ":" + col.ToString();
                        }
                    }
                }
            }

            //}
            //else {
            //    return end.ToString() ;
            //}

            return "-1";
        }

        public async Task<string> SaveGame(string victory, string historysave)
        {
           
            Game game = new Game();
            game.Victory =Convert.ToInt32(victory);
            game.dt = DateTime.Now;
            db.Games.Add(game);

            if (historysave != "")
            {
                string[] masstrhod = historysave.Split('|');
                int count = masstrhod.Length;               // Количество сделанных ходов
                for (int i = 0; i < count; i++)
                {
                    string[] mashod = masstrhod[i].Split('/');
                    //mas[Convert.ToInt32(mashod[1]), Convert.ToInt32(mashod[2])] = Convert.ToInt32(mashod[0]);
                    Hod hod = new Hod();
                    hod.Player = Convert.ToInt32(mashod[0]);
                    hod.Row = Convert.ToInt32(mashod[1]);
                    hod.Col = Convert.ToInt32(mashod[2]);
                    hod.Game = game;
                    db.Hods.Add(hod);                   
                }
                
            }
            await db.SaveChangesAsync();

            return "";
        }

        public async Task<string> LoadGame()
        {
            IEnumerable<Game> games = await db.Games.ToListAsync();

            string str = "";
            foreach (Game g in games)
            {
                string s = "<div>";
                string sh = "";
                foreach (Hod h in g.Hods)
                {
                    if (sh != "") sh = sh + "|";
                    sh = sh + "P" + h.Player + "-" + h.Row + ":" + h.Col;
                }
                if (g.Victory == 0) { s = s + "Ничья";  }
                else { s = s + "Игрок-" + g.Victory; }

                s = s + " - " + g.dt.ToString() + "</br>" + sh + "</div>";
                str = str + s;
            }

            return str;
        }


        int DetectInDiag(int simvol, ref int[,] diag, ref int[,] MassivCell, int count = 2)
        {
            int ret = -1;

            int cs = 0;
            int cn = 0;
            int savei = -1;

            for (int i = 0; i < 3; i++)
            {
                if (MassivCell[diag[i, 0], diag[i, 1]] == simvol)
                {
                    cs++;
                }
                else
                {
                    if (MassivCell[diag[i, 0], diag[i, 1]] == 0)
                    {
                        cn++;
                        savei = i;   //  Запоминаем где находится ячейка без символа
                    }
                    else
                    {
                        return -1;
                    }
                }
            }

            if (cs == count) { ret = savei; }

            return ret;
        }



        /*
        int TestEnd(ref int[,] MassivCell,int count)
        {
            int x = 0;
            for (var n = 0; n <= 2; n++)
            {
                x = MassivCell[n, 0] & MassivCell[n, 1] & MassivCell[n, 2];
                if (x > 0)
                {
                    return x;
                }
                x = MassivCell[0, n] & MassivCell[1, n] & MassivCell[2, n];
                if (x > 0)
                {
                    return x;
                }
            }

            x = MassivCell[0, 0] & MassivCell[1, 1] & MassivCell[2, 2];
            if (x > 0)
            {
                return x;
            }
            x = MassivCell[0, 2] & MassivCell[1, 1] & MassivCell[2, 0];
            if (x > 0)
            {
                return x;
            }

            if (count == 9) { x = 2; }
            return x;
        }
        */
    }
}