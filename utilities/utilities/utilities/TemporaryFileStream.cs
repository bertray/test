///
/// <author>lufty.abdillah@gmail.com</author>
/// <summary>
/// Toyota .Net Development Kit
/// Copyright (c) Toyota Motor Manufacturing Indonesia, All Right Reserved.
/// </summary>
///
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Security.AccessControl;

namespace Toyota.Common.Utilities
{
    public class TemporaryFileStream: FileStream
    {
        public TemporaryFileStream(SafeFileHandle handle, FileAccess access) : base(handle, access) { }
        public TemporaryFileStream(string path, FileMode mode) : base(path, mode) { }
        public TemporaryFileStream(SafeFileHandle handle, FileAccess access, int bufferSize) : base(handle, access, bufferSize) { }
        public TemporaryFileStream(string path, FileMode mode, FileAccess access) : base(path, mode, access) { }
        public TemporaryFileStream(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync) : base(handle, access, bufferSize, isAsync) { }
        public TemporaryFileStream(string path, FileMode mode, FileAccess access, FileShare share) : base(path, mode, access, share) { }
        public TemporaryFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize) : base(path, mode, access, share, bufferSize) { }
        public TemporaryFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync) : base(path, mode, access, share, bufferSize, useAsync) { }
        public TemporaryFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options) : base(path, mode, access, share, bufferSize) { }
        public TemporaryFileStream(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options) : base(path, mode, rights, share, bufferSize, options) { }
        public TemporaryFileStream(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options, FileSecurity fileSecurity) : base(path, mode, rights, share, bufferSize, options, fileSecurity) { }

        public override void Close()
        {
            base.Close();
            File.Delete(Name);
        }
    }
}
