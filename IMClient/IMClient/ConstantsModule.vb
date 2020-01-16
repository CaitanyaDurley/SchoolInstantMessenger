Module ConstantsModule
    'This module contains constants which multiple classes/forms need access to
    Public ReadOnly bannedCharacters As String() = {",", ":", "%", "'"}
    Public Const ServerIP As String = "ENTER SERVER IP HERE"
    'The following are identifiers sent from the client to the server and vice versa in order to identify the command
    Public Const adminString As String = "!$%IM_ADMIN^&*"
    Public Const logOffString As String = "!$%IM_LOGOFF^&*"
    Public Const partnerString As String = "!$%IM_PARTNER^&*"
    Public Const messageString As String = "!$%IM_MESSAGE^&*"
    Public Const logOffForceString As String = "!$%IM_LOGOFFFORCE^&*"
    Public Const friendsPopulateString As String = "!$%IM_FRIENDSPOPULATE^&*"
    Public Const searchFriendsStringGlobal As String = "!$%IM_SEARCHFRIENDS^&*"
    Public Const friendRequestString As String = "!$%IM_FRIENDREQUEST^&*"
    Public Const recommendFriendsString As String = "!$%IM_RCMDFRIENDS^&*"
End Module