using System.IO;
using FlubuCore.Context;

namespace FlubuCore.Tasks.FileSystem
{
    public class CopyFileTask : TaskBase<int>
    {
        private readonly string _destinationFileName;
        private readonly bool _overwrite;

        private readonly string _sourceFileName;

        public CopyFileTask(
            string sourceFileName,
            string destinationFileName,
            bool overwrite)
        {
            _sourceFileName = sourceFileName;
            _destinationFileName = destinationFileName;
            _overwrite = overwrite;
        }

        protected override int DoExecute(ITaskContext context)
        {
            context.LogInfo($"Copy file '{_sourceFileName}' to '{_destinationFileName}'");

            var dir = Path.GetDirectoryName(_destinationFileName);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.Copy(_sourceFileName, _destinationFileName, _overwrite);

            return 0;
        }
    }
}