Public Class findFriendsForm
    Private Sub recommendFriends(sender As Object, e As EventArgs) Handles MyBase.Load
        'Get recommended friends
        Form1.transmission.streamWrite(recommendFriendsString)
        fillFriends()
    End Sub

    Private Sub friendSearchEntry_Click(sender As Object, e As EventArgs) Handles friendSearchEntry.Click
        'When user wishes to enter a friend username delete the hint
        If friendSearchEntry.Text = "Search for a friend" Then
            friendSearchEntry.Clear()
        End If
    End Sub

    Private Sub friendSearchEntry_LostFocus(sender As Object, e As EventArgs) Handles friendSearchEntry.LostFocus
        'When user clicks off the entry replace the hint
        If friendSearchEntry.Text = "" Then
            friendSearchEntry.Text = "Search for a friend"
        End If
    End Sub

    Private Sub findFriendList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles findFriendList.SelectedIndexChanged
        Dim friendRequest As String
        friendRequest = findFriendList.SelectedItem
        'Send friend request to server
        If friendRequest <> "" Then
            If MessageBox.Show(String.Format("Would you like to send a friend request to '{0}'?", friendRequest), "Friend request", MessageBoxButtons.YesNo) = vbYes Then
                Form1.transmission.streamWrite(friendRequestString & friendRequest)
            End If
        End If
    End Sub

    Private Sub friendSearchBtn_Click(sender As Object, e As EventArgs) Handles friendSearchBtn.Click
        'Ensure user enters something
        If friendSearchEntry.Text = "Search for a friend" Then
            MessageBox.Show("Please enter a friend's username", "Enter a username", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            Dim validCreds As Boolean = True
            For Each i In bannedCharacters
                If friendSearchEntry.Text.Contains(i) Then
                    validCreds = False
                    Exit For
                End If
            Next
            If validCreds = True Then
                'Send request to server for usernames like the one entered
                Form1.transmission.streamWrite(searchFriendsStringGlobal & friendSearchEntry.Text)
                findFriendList.Items.Clear()
                fillFriends()
            Else
                MessageBox.Show("Invalid characters found  " & String.Join(" ", bannedCharacters), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub

    Private Sub fillFriends()
        Dim searchFriendsResult As String()
        'Wait for readStream in class transmission in other thread to read the list into searchFriendsString
        While True
            If Form1.transmission.searchFriendsString <> "" Then
                searchFriendsResult = Form1.transmission.searchFriendsString.Split(",")
                Form1.transmission.searchFriendsString = ""
                Exit While
            Else
                Threading.Thread.Sleep(50)
            End If
        End While
        'Display usernames for selection
        For Each i In searchFriendsResult
            If (i <> "" And i <> "NULL") AndAlso (i <> Form1.username And Not Form1.friendList.Items.Contains(i)) Then
                findFriendList.Items.Add(i)
            End If
        Next
    End Sub
End Class