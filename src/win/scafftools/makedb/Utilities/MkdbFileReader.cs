
namespace scafftools.makedb.Utilities
{
    using System;
    using System.IO;

    public class MkdbFileReader : IDisposable
    {
        private readonly string _fileName;

        public MkdbFileReader(string fileName)
        {
            _fileName = fileName;
        }

        public string ReadFileContent()
        {
            if (!File.Exists(_fileName))
                throw new ApplicationException(string.Format("Source file '{0}' not found", _fileName));

            return File.ReadAllText(_fileName);
        }


        #region Implementation of IDisposable

        /// <summary>
        /// Выполняет определяемые приложением задачи, связанные с высвобождением или сбросом неуправляемых ресурсов.
        /// </summary>
        public void Dispose()
        {
            
        }

        #endregion
    }
}
