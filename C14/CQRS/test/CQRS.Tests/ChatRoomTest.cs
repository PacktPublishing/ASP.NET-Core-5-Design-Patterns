using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CQRS.Tests
{
    public class ChatRoomTest
    {
        //private readonly IMediator _mediator = default;
        private readonly TestMessageWriter _reagenMessageWriter = new TestMessageWriter();
        private readonly TestMessageWriter _garnerMessageWriter = new TestMessageWriter();
        private readonly TestMessageWriter _corneliaMessageWriter = new TestMessageWriter();

        private readonly IChatRoom _room1 = new ChatRoom("Room 1");
        private readonly IChatRoom _room2 = new ChatRoom("Room 2");

        private readonly IParticipant _reagen;
        private readonly IParticipant _garner;
        private readonly IParticipant _cornelia;

        public ChatRoomTest()
        {
            var _mediator = new Mediator();
            _mediator.Register(new JoinChatRoom.Handler());
            _mediator.Register(new LeaveChatRoom.Handler());
            _mediator.Register(new SendChatMessage.Handler());
            _mediator.Register(new ListParticipants.Handler());
            _mediator.Register(new ListMessages.Handler());

            _reagen = new Participant(_mediator, "Reagen", _reagenMessageWriter);
            _garner = new Participant(_mediator, "Garner", _garnerMessageWriter);
            _cornelia = new Participant(_mediator, "Cornelia", _corneliaMessageWriter);
        }

        [Fact]
        public void A_participant_should_be_able_to_list_the_participants_that_joined_a_chatroom()
        {
            _reagen.Join(_room1);
            _reagen.Join(_room2);
            _garner.Join(_room1);
            _cornelia.Join(_room2);

            var room1Participants = _reagen.ListParticipantsOf(_room1);
            Assert.Collection(room1Participants,
                p => Assert.Same(_reagen, p),
                p => Assert.Same(_garner, p)
            );
        }

        [Fact]
        public void A_participant_should_be_able_to_send_chat_messages_and_others_should_be_able_to_retrives_them_after_joining()
        {
            _reagen.Join(_room1);
            _reagen.SendMessageTo(_room1, "Hello!");
            _garner.Join(_room1);

            var messages = _garner.ListMessagesOf(_room1);
            Assert.Collection(messages,
                message =>
                {
                    Assert.Same(_reagen, message.Sender);
                    Assert.Equal("Hello!", message.Message);
                }
            );
        }

        [Fact]
        public void A_participant_should_receive_new_messages()
        {
            _reagen.Join(_room1);
            _garner.Join(_room1);
            _garner.Join(_room2);
            _reagen.SendMessageTo(_room1, "Hello!");

            Assert.Collection(_garnerMessageWriter.Output,
                line =>
                {
                    Assert.Equal(_room1, line.chatRoom);
                    Assert.Equal(_reagen, line.message.Sender);
                    Assert.Equal("Hello!", line.message.Message);
                }
            );
        }
    }

    public class TestMessageWriter : IMessageWriter
    {
        public List<(IChatRoom chatRoom, ChatMessage message)> Output { get; } = new List<(IChatRoom, ChatMessage)>();

        public void Write(IChatRoom chatRoom, ChatMessage message)
        {
            Output.Add((chatRoom, message));
        }
    }
}
