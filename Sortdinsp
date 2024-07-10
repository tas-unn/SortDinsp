    public class SortDinSp
    {
        
            // список конкурсов у планов набора
            List<DinSp> ds = new List<DinSp>();
            // Список абитуриентов
            List<Ab> abits = new List<Ab>();
            // флажок
            bool f = false;
            bool local = false; // запускаем на сервере или локально. Влияет на то, откуда берём исходные данные - из файла или из БД

            public void start(string connStr)
            {
                // получаем строку подключения
                //StreamReader sr = new StreamReader("connstr.txt", Encoding.UTF8);
                //string connStr = sr.ReadToEnd();
                //sr.Close();
                // datatable, в которой мы будем хранить данные из sql
                DataTable dt = new DataTable();
                // Создаём и открываем SQL подключение
                SqlConnection conn = new SqlConnection(connStr);
                   
                if (local)
                {
                    //берём из файла
                    StreamReader sr = new StreamReader("dt.json", false);
                    dt = JsonConvert.DeserializeObject<DataTable>(sr.ReadToEnd());
                    sr.Close();
                }
                else
                {
                    conn.Open();
                    StreamReader sr = new StreamReader("get.sql", Encoding.UTF8);
                    string sql = sr.ReadToEnd();
                    sr.Close();
                    // Выполняем SQL запрос
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    // увеличиваем таймаут с 30 секунд до (почти) бесконечности
                    cmd.CommandTimeout = Int32.MaxValue;
                    // Получаем данные и заполняем DataTable
                    SqlDataAdapter DA = new SqlDataAdapter(cmd);
                    DA.Fill(dt);
                
                }

                // текущий id абитуриента. Сначала строка пустая
                string tmpidabit = "";
                // Список заявлений у абитуриента
                List<Statement> statements = new List<Statement>();

                // начинаем заполнение исходных даных
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // если id абитуриента не совпадает (новый абитуриент)
                    if (tmpidabit != dt.Rows[i][0].ToString())
                    {
                        // если не начало получения данных
                        if (tmpidabit != "")
                        {
                            // добавляем абитуриента
                            abits.Add(new Ab(tmpidabit, false, statements));
                            // обнуляем список заявлений
                            statements = new List<Statement>();
                        }
                        // теперь наш новый абитуриент (его id) становится текущим
                        tmpidabit = dt.Rows[i][0].ToString();

                    }

                    List<UInt64> a = new List<UInt64>(); // это лист, который будет использоваться для сортировки
                    for (int j = 5; j < dt.Columns.Count; j++) // пропускаем первые 5 столбца, которые не используются в сортировке
                    {
                        try
                        {
                            a.Add(Convert.ToUInt64(dt.Rows[i][j].ToString()));
                        }
                        catch (Exception ee)
                        {
                            a.Add(0);
                        }
                    }
                    // добавляем все конкурсы
                    ds.Add(new DinSp(dt.Rows[i][1].ToString(), Convert.ToInt32(dt.Rows[i][2].ToString())));
                    // добавляем вакансию для текущего абитуриента
                    statements.Add(new Statement(tmpidabit, dt.Rows[i][1].ToString(), dt.Rows[i][4].ToString(), Convert.ToInt32(dt.Rows[i][3].ToString()), a));
                }
                // добавляем последнего абитуриента
                abits.Add(new Ab(tmpidabit, false, statements));

                // удаляем дубли конкурсов
                ds = ds.GroupBy(elem => elem.konkurs).Select(group => group.First()).ToList();


                // секундомер
                Stopwatch sw = new Stopwatch();
                // количество обходов
                int k = 0;
                // запускаем секундомер
                sw.Start();
                // пока не хорошо, то мы "циклимся"
                while (!f)
                {
                    k++;
                    f = true;
                    // проходимся по всем абитуриентам
                    Task[] tsk = new Task[abits.Count]; // РАСКОММЕНТИРОВАТЬ для параллельных вычислений
                    int i = 0; // РАСКОММЕНТИРОВАТЬ для параллельных вычислений
                    foreach (Ab ab in abits)
                    {
                        // идём по всем его вакансиям
                        tsk[i] = Task.Run(() => useabit(ab));// РАСКОММЕНТИРОВАТЬ для параллельных вычислений
                        i++;// РАСКОММЕНТИРОВАТЬ для параллельных вычислений
                        // useabit(ab); // ЗАКОММЕНТИРОВАТЬ для параллельных вычислений
                    }
                    Task.WaitAll(tsk);// РАСКОММЕНТИРОВАТЬ для параллельных вычислений
                }

                // останавливаем секундомер
                sw.Stop();
                // создаём файл со списками по каждому конкурсу
                //StreamWriter ss = new StreamWriter("json.json", false);
                //ss.WriteLine(JsonConvert.SerializeObject(ds));
                //ss.Close();


                // создаём список абитуриентов с оригиналами, которые не попали в предыдущий файл
                List<Ab> list = new List<Ab>();
                foreach (Ab ab in abits)
                {
                    bool flag = true;
                    foreach (Statement st in ab.statements)
                    {
                        // проходимся по всем спискам, смотрим распределён ли наш абитуриент. Если да, то не проверяем его
                        DinSp nowds = ds.Where(d => d.konkurs == st.konkurs).ToList().FirstOrDefault();
                        List<Statement> newablist = new List<Statement>(nowds.abits);
                        if (newablist.Contains(st))
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag) list.Add(ab);
                }


                list = list.OrderByDescending(a => a.statements[0].a0).ToList();

           
            StreamWriter ss1 = new StreamWriter("ds.json", false);
               ss1.WriteLine(JsonConvert.SerializeObject(ds, Formatting.Indented));
               ss1.Close();
                Console.WriteLine("Количество обходов: " + k.ToString());
                Console.WriteLine("Количество затраченного времени: " + sw.Elapsed.TotalSeconds.ToString());
                // создание файла для саппорта для установки оригиналов
           
                conn.Close();

            }

            void useabit(Ab ab)
            {
                foreach (Statement st in ab.statements)
                {
                    // проходимся по всем спискам, смотрим распределён ли наш абитуриент. Если да, то не проверяем его
                    DinSp nowds = ds.Where(d => d.konkurs == st.konkurs).ToList().FirstOrDefault();
                    List<Statement> newablist = new List<Statement>(nowds.abits);
                    if (newablist.Contains(st))
                        return;
                }
                // проходимся по всем вакансиям абитуриента. Они у нас запросом были отсортированы в нужном порядке
                foreach (Statement st in ab.statements)
                {
                    // находим нужный план набора для этой вакансии
                    DinSp nowds = ds.Where(d => d.konkurs == st.konkurs).ToList().FirstOrDefault();
                    // создаём новый список абитуриентов, копируя текущий
                    List<Statement> newablist = new List<Statement>(nowds.abits);
                    // и в новый список добавляем нашего абитуриента
                    newablist.Add(st);
                    
                    // сортируем новый список
                    // здесь сортировка идёт с помощью LINQ. И всегда по убыванию.
                    // Если нужно где-то по возрастанию, можно перекомпилировать вручную, поставив вместо ThenByDescending ThenBy
                    newablist = newablist.OrderBy(a => a.a0).ThenBy(a => a.a1).ThenByDescending(a => a.a2).ThenByDescending(a => a.a3).ThenByDescending(a => a.a4).ThenByDescending(a => a.a5).ThenByDescending(a => a.a6).ThenByDescending(a => a.a7).ThenByDescending(a => a.a8).ThenByDescending(a => a.a9).ThenByDescending(a => a.a10).ThenByDescending(a => a.a11).ThenByDescending(a => a.a12).ThenByDescending(a => a.a13).ThenByDescending(a => a.a14).ThenByDescending(a => a.a15).ThenByDescending(a => a.a16).ThenByDescending(a => a.a17).ThenByDescending(a => a.a18).ThenByDescending(a => a.a19).ToList();
                    // удаляем абитуриентов из нового списка, не попавших в список из-за кол-ва бюджетных мест
                    // нас сейчас не волнует это наш текущий абитуриент, либо какой-то, кто ранее распределён
                    while (newablist.Count > nowds.places)
                        newablist.RemoveAt(newablist.Count - 1);
                    // это списки для сравнения между собой.
                    // Найдено https://stackoverflow.com/questions/12795882/quickest-way-to-compare-two-generic-lists-for-differences тут. Работает и ладно
                    var firstNotSecond = nowds.abits.Except(newablist).ToList();
                    var secondNotFirst = newablist.Except(nowds.abits).ToList();
                    if (!(!firstNotSecond.Any() && !secondNotFirst.Any()))
                    {
                        // если не совпадают, то мы будем ещё раз циклиться в while'e и перепроходить всех абитуриентов заново
                        f = false;
                        // делаем наш новый список текущим
                        nowds.abits = newablist;
                        // если мы сейчас распределили нашего абитуриента, то дальше мы его не проверяем
                        if (newablist.Contains(st))
                            break;
                    }
                }


            }
        }
