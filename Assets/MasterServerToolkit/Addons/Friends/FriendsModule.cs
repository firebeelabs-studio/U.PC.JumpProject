using MasterServerToolkit.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MasterServerToolkit.MasterServer
{
    public class FriendsModule : BaseServerModule
    {
        #region INSPECTOR

        [Header("General Settings")]
        [SerializeField, Tooltip("If true, this module will subscribe to auth module, and automatically setup users when they log in")]
        protected bool useAuthModule = true;

        /// <summary>
        /// Database accessor factory that helps to create integration with friends db
        /// </summary>
        [Header("Components"), Tooltip("Database accessor factory that helps to create integration with friends db"), SerializeField]
        protected DatabaseAccessorFactory friendsAccessorFactory;

        #endregion

        /// <summary>
        /// Auth module for listening to auth events
        /// </summary>
        protected AuthModule authModule;

        /// <summary>
        /// Chat module for listening to chat events
        /// </summary>
        protected ChatModule chatModule;

        /// <summary>
        /// List of friends
        /// </summary>
        private Dictionary<string, HashSet<string>> friends;

        /// <summary>
        /// 
        /// </summary>
        private IFriendsDatabaseAccessor friendsDatabaseAccessor;

        public override void Initialize(IServer server)
        {
            friendsAccessorFactory?.CreateAccessors();

            // Get auth module dependency
            authModule = server.GetModule<AuthModule>();

            //TODO Get chat module dependency. Use this to add friends to private chat chennel
            chatModule = server.GetModule<ChatModule>();

            if (useAuthModule)
            {
                if (authModule)
                {
                    authModule.OnUserLoggedInEvent += OnUserLoggedInEventHandler;
                }
                else
                {
                    logger.Error($"{GetType().Name} was set to use {nameof(AuthModule)}, but {nameof(AuthModule)} was not found");
                }
            }

            friendsDatabaseAccessor = Mst.Server.DbAccessors.GetAccessor<IFriendsDatabaseAccessor>();

            server.RegisterMessageHandler(MstOpCodes.GetFriends, GetFriendsMessageHandler);

            //server.RegisterMessageHandler((ushort)MstOpCodes.RequestFriendship, RequestFriendshipHandler);
            //server.RegisterMessageHandler((ushort)MstOpCodes.AcceptFriendship, AcceptFriendshipHandler);
            //server.RegisterMessageHandler((ushort)MstOpCodes.DeclineFriendship, DeclineFriendshipHandler);
            //server.RegisterMessageHandler((ushort)MstOpCodes.GetDeclinedFriendships, GetDeclinedFriendshipsHandler);


            //server.RegisterMessageHandler((ushort)MstOpCodes.InspectFriend, InspectFriendHandler);
            //server.RegisterMessageHandler((ushort)MstOpCodes.BlockFriends, BlockFriendsHandler);
            //server.RegisterMessageHandler((ushort)MstOpCodes.RemoveFriends, RemoveFriendsHandler);
        }

        public override MstProperties Info()
        {
            MstProperties info = base.Info();
            info.Set("Description", "This is a friends module that helps users to make friendship requests, accept friendships, and receive a list of friends.");
            info.Add("Database Accessor", friendsAccessorFactory != null ? "Connected" : "Not Connected");
            info.Add("Users with friends", friends.Count);
            return info;
        }

        /// <summary>
        /// Invoked when user logs in
        /// </summary>
        /// <param name="user"></param>
        protected virtual void OnUserLoggedInEventHandler(IUserPeerExtension user)
        {

        }

        #region MESSAGE HANDLERS

        private void GetFriendsMessageHandler(IIncomingMessage message)
        {
            try
            {
                var userExtension = message.Peer.GetExtension<IUserPeerExtension>();

                if (userExtension == null)
                {
                    message.Respond(ResponseStatus.Unauthorized);
                    return;
                }


            }
            catch (Exception e)
            {
                logger.Error(e);
                message.Respond(ResponseStatus.Error);
            }
        }

        #endregion
    }
}