Public Class adminForm
    'Commands only the admin can send to the server
    Private Const adminGetUsersString As String = "!$%IM_GETUSERS^&*"
    Private Const adminDeleteUserString As String = "!$%IM_DELUSER^&*"
    Private Const adminGetMessageString As String = "!$%IM_GETMESSAGE^&*"
    Private Const adminChangePasswordString As String = "!$%IM_CHNGPSSWD^&*"
    Private Const adminGetFriendRequestString As String = "!$%IM_GETFRNDRQST^&*"
    Private Const adminAcceptFriendRequestString As String = "!$%IM_ACPTFRNDRQST^&*"
    Private Const adminRejectFriendRequestString As String = "!$%IM_RJCTFRNDRQST^&*"

    Private Sub getUsersAndFriendRequests(sender As Object, e As EventArgs) Handles MyBase.Load
        'Get list of users
        Form1.transmission.streamWrite(adminGetUsersString)
        Dim getUsersResult As String()
        'Wait for readStream in class transmission in other thread to read the list into searchFriendsString
        While True
            If Form1.transmission.searchFriendsString <> "" Then
                getUsersResult = Form1.transmission.searchFriendsString.Split(",")
                Form1.transmission.searchFriendsString = ""
                Exit While
            Else
                Threading.Thread.Sleep(50)
            End If
        End While
        'Display users
        For Each i In getUsersResult
            If i <> "" And i <> Form1.username Then
                userListBox.Items.Add(i)
            End If
        Next
        'Use friendsPopString to get unapproved friend requests
        Form1.transmission.streamWrite(adminGetFriendRequestString)
        Dim tmpFriendList As String()
        While True
            If Form1.transmission.friendsPopString <> "" Then
                tmpFriendList = Form1.transmission.friendsPopString.Split(",")
                Form1.transmission.friendsPopString = ""
                Exit While
            Else
                Threading.Thread.Sleep(50)
            End If
        End While

        For i = 1 To tmpFriendList.Count - 1 Step 2
            If tmpFriendList(i) <> "" And tmpFriendList(i) <> "NULL" Then
                friendRequestListBox.Items.Add(tmpFriendList(i) & "," & tmpFriendList(i + 1))
            End If
        Next
    End Sub

    Private Sub requestAcceptBtn_Click(sender As Object, e As EventArgs) Handles requestAcceptBtn.Click
        'Accept friend request
        If friendRequestListBox.SelectedItem <> "" Then
            Form1.transmission.streamWrite(adminAcceptFriendRequestString & friendRequestListBox.SelectedItem)
            friendRequestListBox.Items.Remove(friendRequestListBox.SelectedItem)
        End If
    End Sub

    Private Sub requestRejectBtn_Click(sender As Object, e As EventArgs) Handles requestRejectBtn.Click
        'Reject friend request
        If friendRequestListBox.SelectedItem <> "" Then
            Form1.transmission.streamWrite(adminRejectFriendRequestString & friendRequestListBox.SelectedItem)
            friendRequestListBox.Items.Remove(friendRequestListBox.SelectedItem)
        End If
    End Sub

    Private Sub viewMsgBtn_Click(sender As Object, e As EventArgs) Handles viewMsgBtn.Click
        If userListBox.SelectedItems.Count = 2 Then
            Form1.listView.Items.Clear()
            Form1.friendList.ClearSelected()
            Form1.partner = ""
            'Ask server to send user's message, will be displayed in Form1
            Form1.transmission.streamWrite(adminGetMessageString & userListBox.SelectedItems(0) & "," & userListBox.SelectedItems(1))
            Close()
        Else
            MessageBox.Show("Please select two users", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub passChangeBtn_Click(sender As Object, e As EventArgs) Handles passChangeBtn.Click
        Dim password As String = passChangeEntry.Text
        If password <> "" And password <> "Enter new password..." Then
            If userListBox.SelectedItems.Count = 1 Then
                'Change a user's password
                Form1.transmission.streamWrite(adminChangePasswordString & userListBox.SelectedItem & "," & password)
                passChangeEntry.Text = "Enter new password..."
            Else
                MessageBox.Show("Please select one user", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            MessageBox.Show("Please enter the new password", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub passChangeEntry_Click(sender As Object, e As EventArgs) Handles passChangeEntry.Click
        'When user wishes to write a message delete the hint
        If passChangeEntry.Text = "Enter new password..." Then
            passChangeEntry.Clear()
        End If
    End Sub

    Private Sub passChangeEntry_LostFocus(sender As Object, e As EventArgs) Handles passChangeEntry.LostFocus
        'When user clicks off the entry replace the hint
        If passChangeEntry.Text = "" Then
            passChangeEntry.Text = "Enter new password..."
        End If
    End Sub

    Private Sub delUserBtn_Click(sender As Object, e As EventArgs) Handles delUserBtn.Click
        If userListBox.SelectedItems.Count = 1 Then
            'Delete user
            If MessageBox.Show("Would you like to remove this account? All data will be lost", "Remove user", MessageBoxButtons.YesNo) = vbYes Then
                Form1.transmission.streamWrite(adminDeleteUserString & userListBox.SelectedItem)
                userListBox.Items.Remove(userListBox.SelectedItem)
            End If
        Else
            MessageBox.Show("Please select one user", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
End Class