using Aran.Temporality.Common.Aim.Extension.Message;
using Aran.Temporality.Internal.Util;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TemporalityTest
{
    public class FContainerFileTest
    {
        
        [Fact]
        public void CheckWriteContainerSuccess()
        {
            var fcontainer = new FContainer<object>();
            fcontainer.Object = new object();

            var stream = new MemoryStream();
            var result = FContainerFile<object>.WriteContainer(fcontainer, stream);

            Assert.True(result);
        }

        //[Fact]
        //public void CheckReadContainerSuccess()
        //{
        //    var fContainer = Mock.Of<FContainer<object>>();

        //    var stream = new MemoryStream();
        //    var result = FContainerFile<object>.WriteContainer(fContainer, stream);

        //    //Mock.Get(fContainer).Verify(x => x.Id,Times.);
        //    Mock.Get(fContainer).Verify(x => x.Size, Times.Exactly(1));
        //    Mock.Get(fContainer).Verify(x => x.Delete, Times.Exactly(1));
        //    Mock.Get(fContainer).Verify(x => x.GetSize(), Times.AtMost(2));
        //}

        [Fact]
        public void CheckReadAndWriteIsSame()
        {
            var fcontainer = new FContainer<UserExtension>();
            fcontainer.Delete = false;
            var userExtension = new UserExtension {Application="Panda",User="Agshin" };
            fcontainer.Object = userExtension;

            var stream = new MemoryStream();
            var writeResult = FContainerFile<UserExtension>.WriteContainer(fcontainer, stream);
            Assert.True(writeResult);

            stream.Position = 0;
           // var readStream = new MemoryStream(fcontainer.Data);
            var popUserExtension =  FContainerFile<UserExtension>.GetNextContainer(stream);

            Assert.Equal(userExtension.Application, (popUserExtension.Object as UserExtension).Application);
            Assert.Equal(userExtension.User, (popUserExtension.Object as UserExtension).User);

        }


    }
}
