using System;
using Aran.Temporality.Common.Entity;
using Xunit;

namespace Toss.Tests.NoAixm
{
    [Collection(nameof(Names.ServiceCollection))]
    [TestCaseOrderer("Toss.Tests.PriorityOrderer", "Toss.Tests")]
    public class NoAixmDataServiceTests: IDisposable
    {
        private readonly DataFixture _dataFixture;

        public NoAixmDataServiceTests(DataFixture dataFixture)
        {
            _dataFixture = dataFixture;
        }

        [Fact, TestPriority(0)]
        public void CreatePublicSlot()
        {
            DateTime date = _dataFixture.NextAiracCycle;
            PublicSlot publicSlot = new PublicSlot()
            {
                Name = $"Public_{Guid.NewGuid()}",
                SlotType = 0,
                EffectiveDate = date
            };
            int id = _dataFixture.CreatePublicSlot(publicSlot); 
            var slot = _dataFixture.NoAixmDataService.GetPublicSlotById(id);
            Assert.NotNull(slot);
            Assert.Equal(slot.Id, publicSlot.Id);
            Assert.Equal(slot.SlotType, publicSlot.SlotType);
            Assert.Equal(slot.Name, publicSlot.Name);
            Assert.Equal(slot.EffectiveDate, publicSlot.EffectiveDate);
            Assert.Equal(slot.EndEffectiveDate, publicSlot.EndEffectiveDate);
        }

        [Fact, TestPriority(1)]
        public void CreatePrivateSlot()
        {
            int id = _dataFixture.CreatePublicSlot(true);
            PrivateSlot privateSlot = new PrivateSlot()
            {
                PublicSlot = _dataFixture.NoAixmDataService.GetPublicSlotById(id),
                Name = $"Private_{Guid.NewGuid()}"
            };
            var privateSlotId = _dataFixture.CreatePrivateSlot(privateSlot);
            var slot = _dataFixture.NoAixmDataService.GetPrivateSlotById(privateSlotId);
            Assert.NotNull(slot);
            Assert.Equal(slot.Id, privateSlot.Id);
            Assert.Equal(slot.PublicSlot.Id, privateSlot.PublicSlot.Id);
            Assert.Equal(slot.Name, privateSlot.Name);
        }

        public void Dispose()
        {
            _dataFixture.DeleteSlots();
        }
    }
}
