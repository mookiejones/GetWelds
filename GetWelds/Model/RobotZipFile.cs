using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace GetWelds.ViewModels
{
    public class RobotZipFile : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private ZipFile _zip;


        public string Name { get { return _zip.Name; } }

        #region · Constructors ·

        public RobotZipFile()
        {
        }


        public RobotZipFile(Encoding encoding)
        {
            _zip = new ZipFile(encoding);
        }

        public RobotZipFile(string fileName)           
        {
            _zip = new ZipFile(fileName);
            Initialize();
        }

        public RobotZipFile(string fileName, Encoding encoding)
        {
            _zip = new ZipFile(fileName, encoding);
            Initialize();
        }

        public RobotZipFile(string fileName, TextWriter statusMessageWriter)
           
        {
            _zip = new ZipFile(fileName, statusMessageWriter);
            Initialize();
        }

        public RobotZipFile(string fileName, TextWriter statusMessageWriter, Encoding encoding)
            
        {
            _zip = new ZipFile(fileName, statusMessageWriter, encoding);
            Initialize();
        }

        #endregion · Constructors ·

        #region · Properties ·

        #region Date

        /// <summary>
        /// The <see cref="Date" /> property's name.
        /// </summary>
        public const string DATE_PROPERTY_NAME = "Date";

        private DateTime _date = default(DateTime);

        /// <summary>
        /// Sets and gets the Date property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public DateTime Date
        {
            get
            {
                return _date;
            }

            set
            {
                if (_date == value)
                {
                    return;
                }

                RaisePropertyChanging(DATE_PROPERTY_NAME);
                _date = value;
                RaisePropertyChanged(DATE_PROPERTY_NAME);
            }
        }

        #endregion Date

        #region Path

        /// <summary>
        /// The <see cref="Path" /> property's name.
        /// </summary>
        public const string PATH_PROPERTY_NAME = "Path";

        private string _path = string.Empty;

        /// <summary>
        /// Sets and gets the Path property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Path
        {
            get
            {
                return _path;
            }

            set
            {
                if (_path == value)
                {
                    return;
                }

                RaisePropertyChanging(PATH_PROPERTY_NAME);
                _path = value;
                RaisePropertyChanged(PATH_PROPERTY_NAME);
            }
        }

        #endregion Path

        #endregion · Properties ·

        #region · Private Methods ·

        private void Initialize()
        {
            var fi = new FileInfo(_zip.Name);
            Path = _zip.Name;
            Date = fi.LastWriteTime;
        }

        #endregion · Private Methods ·

        #region · Public Methods ·

        public ICollection<ZipEntry> Entries { get { return _zip.Entries; } }
        public void ExtractAll(string path,ExtractExistingFileAction extractExistingFile)
        {


            Console.WriteLine("Extracting {0}", path);
            if (Directory.Exists(path))
            {
                var access = Directory.GetAccessControl(path);
                try
                {
                    File.SetAttributes(path, FileAttributes.Normal);
                    Directory.Delete(path, true);

                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.ToString());
                }
            }

            _zip.ExtractProgress += _zip_ExtractProgress;
            _zip.ExtractAll(path,ExtractExistingFileAction.InvokeExtractProgressEvent);
        }

        private void _zip_ExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            switch(e.EventType)
            {
                case ZipProgressEventType.Extracting_BeforeExtractAll:
                case ZipProgressEventType.Extracting_AfterExtractEntry:
                case ZipProgressEventType.Extracting_BeforeExtractEntry:
                case ZipProgressEventType.Extracting_EntryBytesWritten:
                case ZipProgressEventType.Extracting_AfterExtractAll:
                    break;
                case ZipProgressEventType.Extracting_ExtractEntryWouldOverwrite:
                    e.CurrentEntry.Extract(ExtractExistingFileAction.OverwriteSilently);

                    break;
            }
        }


        public ICollection<string> EntryFileNames { get { return _zip.EntryFileNames; } }
        public string GetFileText(ZipEntry entry)
        {

            var temp = System.IO.Path.GetTempPath();
            var name = System.IO.Path.Combine(temp, entry.FileName);
            entry.Extract(temp,ExtractExistingFileAction.OverwriteSilently);
            var text = File.ReadAllText(name);
            File.Delete(name);
            return text;
        }


        public ZipEntry GetZipEntry(string filename)
        {
            return _zip.Entries.FirstOrDefault(f => f.FileName.ToLowerInvariant().Contains(filename.ToLowerInvariant()));
        }

        public   string[] ReadEntryLines(ZipEntry entry)
        {
            var list = new List<string>();
            using (var reader = new StreamReader(entry.OpenReader()))
            {
                while (!reader.EndOfStream)
                    list.Add(reader.ReadLine());
            }
                        return list.ToArray();

        }
        public  string ReadEntryText(ZipEntry entry)
        {
            using (var reader = new StreamReader(entry.OpenReader()))
                return reader.ReadToEnd();
        }


        public string GetFileText(string filePath)
        {
            var file = _zip.Entries.FirstOrDefault(f => f.FileName.ToLower().Contains(filePath));

            if (file == null)
                return string.Empty;


            var tempPath = System.IO.Path.GetTempPath();
            var sr = new StreamReader(file.OpenReader());
            return sr.ReadToEnd();

        }

        public string[] GetFileLines(string filename)
        {

            var file = _zip.Entries.FirstOrDefault(f => f.FileName.Contains(filename));
            return ReadEntryLines(file);
        }

        #endregion · Public Methods ·

        #region · Notify Property Changed Implementation ·

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void RaisePropertyChanging(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }

        #endregion · Notify Property Changed Implementation ·
    }
}