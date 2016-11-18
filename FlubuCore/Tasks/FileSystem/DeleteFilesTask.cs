using System.IO;
using FlubuCore.Context;

namespace FlubuCore.Tasks.FileSystem
{
    public class DeleteFilesTask : TaskBase<int>
    {
        private readonly string _directoryPath;
        private readonly string _filePattern;
        private readonly bool _recursive;

        public DeleteFilesTask(string directoryPath, string filePattern, bool recursive)
        {
            _directoryPath = directoryPath;
            _filePattern = filePattern;
            _recursive = recursive;
        }

        public static void Execute(
            ITaskContext context,
            string directoryPath,
            string filePattern,
            bool recursive)
        {
            var task = new DeleteFilesTask(directoryPath, filePattern, recursive);
            task.ExecuteVoid(context);
        }

        protected override int DoExecute(ITaskContext context)
        {
            context.LogInfo($"Delete files from directory {_directoryPath} matching pattern '{_filePattern}'");

            var searchOption = SearchOption.TopDirectoryOnly;

            if (_recursive)
                searchOption = SearchOption.AllDirectories;

            foreach (var file in Directory.EnumerateFiles(_directoryPath, _filePattern, searchOption))
            {
                File.Delete(file);
                context.LogInfo($"Deleted file '{file}'");
            }

            return 0;
        }
    }
}