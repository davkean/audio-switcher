// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

using System;
using System.IO;
using AudioSwitcher.Presentation.Drawing;
using Xunit;

namespace AudioSwitcher.IO
{
    [Trait("", "UnitTests")]
    public class StreamExtensionsTests
    {
        [Fact]
        public void Write_NullAsStream_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("stream", () =>
            {
                StreamExtensions.Write(null, default(int));
            });
        }

        [Fact]
        public void Read_NullAsStream_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("stream", () =>
            {
                StreamExtensions.Read<int>(null);
            });
        }               
      
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void ReadWrite_CanRoundtripInt32(int value)
        {
            var stream = new MemoryStream();

            StreamExtensions.Write(stream, value);

            stream.Position = 0;

            var result = StreamExtensions.Read<int>(stream);

            Assert.Equal(value, result);
        }

        [Fact]
        public void ReadWrite_CanRoundtripStruct()
        {
            var value = new IconDir()
            {
                Reserved = 0,
                Type = 1,
                Count = 1,
            };

            var stream = new MemoryStream();

            StreamExtensions.Write(stream, value);

            stream.Position = 0;

            var result = StreamExtensions.Read<IconDir>(stream);

            Assert.Equal(value, result);
        }
    }
}
