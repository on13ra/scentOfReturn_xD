namespace scentOfReturn_xD.StoreTxts
{
    public class LineStorage
    {
        private List<Line> _list;

        public List<Line> TheList
        {
            get { return _list; }
            set { _list = value; }
        }

        private int _lineCount;

        public int LineCount
        {
            get { return _lineCount; }
            set { _lineCount = value; }
        }


        public async void FillList()
        {
            String vline;
            Line line = new();
            try
            {
                using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync("rasp.txt");
                using StreamReader sr = new StreamReader(fileStream);
                while ((vline = sr.ReadLine()) != null)
                {
                    line = new Line();
                    line.TheLine = vline;
                    if (line.TheLine.Length < 4)
                    {
                        line.TheLine = "нет пары";
                        _list.Add(line);
                    }
                    else _list.Add(line);
                }
            }
            catch (Exception e)
            {
                _lineCount = -1;
                Line error = new Line();
                error.TheLine = e.ToString();
                _list.Add(error);
            }
        }

        public LineStorage()
        {
            _list = new List<Line>();
            FillList();
            _lineCount = _list.Count;
            if (_list == null)
            {
                _list = new List<Line>();
                FillList();
                _lineCount = _list.Count;
            }
        }
    }
}
