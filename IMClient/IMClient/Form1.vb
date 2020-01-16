Imports System.Net
Imports System.Net.Sockets
Imports System.IO
Imports System.Security.Cryptography

Public Class Form1
    Private ReadOnly profanityList As String() = {"muffin", "bannana", "giraffe", "hooligan"}
    'How many characters can fit in the message box
    Private Const listViewSize As Integer = 140
    Property transmission
    Property loggedIn As Boolean = False
    Property partner As String
    'Shared because different forms and classes need access
    Public Shared Property username As String
    Public Shared Property minimised As Boolean = False
    Property admin As Boolean = False

    Private Sub msgEntry_Click(sender As Object, e As EventArgs) Handles msgEntry.Click
        'When user wishes to write a message delete the hint
        If msgEntry.Text = "Write your message here..." Then
            msgEntry.Clear()
        End If
    End Sub

    Private Sub msgEntry_LostFocus(sender As Object, e As EventArgs) Handles msgEntry.LostFocus
        'When user clicks off the entry replace the hint
        If msgEntry.Text = "" Then
            msgEntry.Text = "Write your message here..."
        End If
    End Sub

    Private Sub Submit_GotFocus(sender As Object, e As EventArgs) Handles Submit.MouseEnter
        'User can only click if logged in
        If loggedIn = True And partner <> "" Then
            Submit.Cursor = Cursors.Default
        Else
            Submit.Cursor = Cursors.No
        End If
    End Sub

    Private Sub Submit_Click(sender As Object, e As EventArgs) Handles Submit.Click
        If loggedIn = False Then
            MessageBox.Show("Please log in", "Log in", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf partner = "" Then
            MessageBox.Show("Please select a friend", "Select a friend", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf msgEntry.Text = "Write your message here..." Then
            MessageBox.Show("Please enter a message", "Enter a message", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            'Ensure user's message is clean
            If profanityDetector(msgEntry.Text) = False Then
                Dim message As String = username & ": " & msgEntry.Text
                'Send user's message to streamWrite function and add message to the list
                transmission.streamWrite(messageString & message)
                addItemsListView(New String() {message}, False)
                msgEntry.Clear()
            Else
                MessageBox.Show("Profanity detected", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub

    Private Function profanityDetector(ByVal message As String) As Boolean
        For Each i In profanityList
            'See if user's message contains a profanity anywhere in the string
            If message.Contains(i) Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub PressedX() Handles Me.FormClosing
        If loggedIn = True Then
            logIn_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub logIn_Click(sender As Object, e As EventArgs) Handles logIn.Click
        Dim validCreds As Boolean = True
        If loggedIn = True Then
            'Sends log out code to server and shuts down
            loggedIn = False
            loggedLbl.Text = "Logged out"
            usernameLbl.Text = ""
            logIn.Text = "Log in"
            friendList.Items.Clear()
            onlineList.Items.Clear()
            listView.Items.Clear()
            friendList.ClearSelected()
            findFriendsBtn.Text = "Find friends"
            transmission.shutdown()
        Else
            'Try logging in
            Dim dist As New distAndAuth
            Dim port As Integer
            Dim rsa As New RSA
            Dim aes As New AES
            If userEntry.Text <> "" And passEntry.Text <> "" Then
                'Ensures doesn't send nothing to server
                'Check user's details doesn't contain banned characters
                For Each i In bannedCharacters
                    If userEntry.Text.Contains(i) Or passEntry.Text.Contains(i) Then
                        validCreds = False
                        Exit For
                    End If
                Next
                If validCreds = True Then
                    'Authenticate with server and get port
                    port = dist.getPort(userEntry.Text, passEntry.Text, newAccount.Checked, rsa, aes)
                    If port > 1 Then
                        'Setup transmission
                        transmission = New transmission(port, aes)
                        loggedIn = True
                        loggedLbl.Text = "Logged In"
                        logIn.Text = "Log out"
                        username = userEntry.Text
                        usernameLbl.Text = "Logged in as " & username
                        userEntry.Clear()
                        passEntry.Clear()
                        'Client must always be reading from stream so start this in new thread so gui still responsive
                        Dim t As New Threading.Thread(
                        Sub()
                            transmission.streamRead(Me)
                        End Sub
                    )
                        t.Start()
                        'Populate friends list
                        friendsPopulate(Nothing, Nothing)
                        friendList.ClearSelected()
                        partner = ""
                    ElseIf port = 0 Then
                        'Incorrect login details/username taken
                        loggedLbl.Text = "Incorrect details"
                        If newAccount.Checked = True Then
                            MessageBox.Show("Username already taken", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            MessageBox.Show("Incorrect details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    ElseIf port = 1 Then
                        'Server at max capacity
                        loggedLbl.Text = "Server busy"
                    End If
                Else
                    MessageBox.Show("Invalid characters found  " & String.Join(" ", bannedCharacters), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
        End If
    End Sub

    Private Sub friendList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles friendList.SelectedIndexChanged
        If loggedIn = True And friendList.SelectedItem <> "" And friendList.SelectedItem <> partner Then
            'Change partner as long as user hasn't mis-clicked
            partner = friendList.SelectedItem
            'Tell server partner changed
            transmission.streamWrite(partnerString & partner)
            listView.Items.Clear()
            friendList.BorderStyle = BorderStyle.None
        End If
    End Sub

    Private Sub friendsPopulate(sender As Object, e As EventArgs) Handles friendListBtn.Click
        If loggedIn = False Then
            Return
        End If
        'Disable button so user doesn't spam it
        friendListBtn.Enabled = False
        Dim tmpFriendList As String()
        Dim tmp(1) As String
        'Tell server to send list of friends
        transmission.streamWrite(friendsPopulateString)
        'Wait for readStream in class transmission in other thread to read the list into friendsPopString
        While True
            If transmission.friendsPopString <> "" Then
                tmpFriendList = transmission.friendsPopString.Split(",")
                transmission.friendsPopString = ""
                Exit While
            Else
                Threading.Thread.Sleep(50)
            End If
        End While

        friendList.BeginUpdate()
        onlineList.BeginUpdate()
        friendList.Items.Clear()
        onlineList.Items.Clear()
        For Each i In tmpFriendList
            'Split from username:True/False
            tmp = i.Split(":")
            If tmp(0) <> "" And tmp(0) <> "NULL" Then
                If tmp(1) = "True" Then
                    friendList.Items.Add(tmp(0))
                    onlineList.Items.Add("✔")
                Else
                    friendList.Items.Add(tmp(0))
                    onlineList.Items.Add("✘")
                End If
            End If
        Next
        friendList.EndUpdate()
        onlineList.EndUpdate()
        friendList.SelectedItem = partner
        friendListBtn.Enabled = True

        'Check if user is admin. Put here because first action by client is friendsPopulate
        If transmission.admin = True Then
            findFriendsBtn.Text = "Admin"
            admin = True
        End If
    End Sub

    Private Sub friendListBtn_GotFocus(sender As Object, e As EventArgs) Handles friendListBtn.MouseEnter
        'User can only click if logged in
        If loggedIn = True Then
            friendListBtn.Cursor = Cursors.Default
        Else
            friendListBtn.Cursor = Cursors.No
        End If
    End Sub

    Private Sub findFriendsBtn_Click(sender As Object, e As EventArgs) Handles findFriendsBtn.Click
        'Display either findFriends or admin form depending on if user is admin. Admin doesn't need findFriends form because is automatically friended with everybody
        If loggedIn = True Then
            If admin = False Then
                findFriendsForm.Show()
            Else
                adminForm.Show()
            End If
        Else
            MessageBox.Show("Please log in", "Log in", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub Form1_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        'The class transmission needs to know if the form is minimised so it knows whether to create a notification or not
        If WindowState = 1 Then
            minimised = True
        Else
            minimised = False
        End If
    End Sub

    Public Sub addItemsListView(ByVal items As String(), ByVal adminOverride As Boolean)
        'If transmission calls this sub then an invoke will be needed as GUI operations can only be called from the GUI thread
        If listView.InvokeRequired Then
            listView.Invoke(Sub() addItemsListView(items, adminOverride))
        Else
            Dim index As Integer
            Dim name As String
            Dim isPartner As Boolean = False
            For Each item In items
                Dim lvi As New ListViewItem
                lvi.ForeColor = Color.Navy
                name = item.Substring(0, item.IndexOf(":") + 2)
                If name = partner & ": " Then
                    isPartner = True
                End If
                If isPartner = True Or name = username & ": " Or adminOverride = True Then
                    'Checks whether the message will fit in listView
                    If item.Length > listViewSize Then
                        'Gets index of the last space that fits on one line, so the sentence can be split over two lines between words
                        index = item.Substring(name.Length, listViewSize - name.Length).LastIndexOf(" ") + name.Length
                        If index = name.Length - 1 Then
                            'The message has no spaces so just put in what will fit
                            index = listViewSize - 1
                        End If
                        lvi.Text = item.Substring(0, index)
                        If isPartner = True Then
                            'Change colour if is a different user
                            lvi.ForeColor = Color.DarkRed
                        End If
                        'Insert what's been decided on
                        listView.Items.Add(lvi)
                        'Recursively call on what is left of the string
                        addItemsListView(New String() {name & item.Substring(index + 1)}, adminOverride)
                    Else
                        'The string will fit in one line
                        lvi.Text = item
                        If isPartner = True Then
                            lvi.ForeColor = Color.DarkRed
                        End If
                        listView.Items.Add(lvi)
                    End If
                    'Scroll to last message
                    listView.EnsureVisible(listView.Items.Count - 1)
                Else
                    'If a message is received that is not from the selected partner, let the user know
                    friendList.Invoke(Sub() friendList.BorderStyle = BorderStyle.Fixed3D)
                End If
                'Reset isPartner before next iteration
                isPartner = False
            Next
        End If
    End Sub
End Class

Class transmission
    Private tcpClient As TcpClient = New TcpClient
    Private stream As NetworkStream
    Property friendsPopString As String = ""
    Property searchFriendsString As String = ""
    Property admin As Boolean = False
    Private aes As AES

    Public Sub New(ByVal port As Integer, ByVal aesp As AES)
        Try
            'Connect on supplied port, and assign values to stream and aes 
            tcpClient.Connect(ServerIP, port)
            stream = tcpClient.GetStream()
            aes = aesp
        Catch
            MessageBox.Show("Sorry, something went wrong", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End
        End Try
    End Sub

    Public Sub shutdown()
        streamWrite(logOffString)
        stream.Close()
        tcpClient.Close()
    End Sub

    Public Sub streamWrite(ByVal message As String)
        Dim data As Byte()
        data = aes.encrypt(message)
        stream.Write(data, 0, data.Length)
    End Sub

    Public Sub streamRead(ByVal myForm1 As Form1)
        Dim localPartner As String
        Dim notification As New NotifyIcon With {.Visible = True, .Icon = SystemIcons.Application, .BalloonTipIcon = ToolTipIcon.Info}
        Dim recvString As String
        Dim tmpBytes(1023) As Byte
        Dim length As Integer

        Do
            Try
                'Transfers the bytes transmitted from server into a byte array and gets length of data received
                length = stream.Read(tmpBytes, 0, tmpBytes.Length)

                'Array of correct size created with only data received copied in
                Dim recvBytes(length - 1) As Byte
                Array.Copy(tmpBytes, recvBytes, length)

                'Convert into plaintext
                recvString = aes.decrypt(recvBytes)

                If recvString.StartsWith(messageString) Then
                    'Get actual message
                    recvString = recvString.Substring(16)
                    localPartner = recvString.Split(":")(0)
                    'Display it
                    myForm1.addItemsListView(New String() {recvString}, False)

                    If Form1.minimised = True Then
                        'Create notification for received message if the window is minimised
                        notification.BalloonTipTitle = "New message from " & localPartner
                        notification.BalloonTipText = recvString.Split(":")(1)
                        notification.ShowBalloonTip(10000)
                    End If

                ElseIf recvString.StartsWith(partnerString) Then
                    recvString = recvString.Substring(16)
                    'Display the messages to and from this user
                    myForm1.addItemsListView(Split(recvString, partnerString), admin)
                ElseIf recvString.StartsWith(friendsPopulateString) Then
                    'Put the list of friends into friendsPopString, where it will be read from by Form1
                    friendsPopString = recvString.Substring(24)
                    If friendsPopString = "" Then
                        friendsPopString = ",NULL:NULL"
                    End If
                ElseIf recvString.StartsWith(searchFriendsStringGlobal) Then
                    searchFriendsString = recvString.Substring(22)
                    If searchFriendsString = "" Then
                        searchFriendsString = ",NULL"
                    End If
                ElseIf recvString = adminString Then
                    admin = True
                ElseIf recvString = logOffForceString Then
                    MessageBox.Show("You have been disconnected", "Duplicate connection", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End
                End If
            Catch
                Exit Do
            End Try
        Loop
    End Sub
End Class

Class distAndAuth
    Private Const distributePort As Integer = 1024
    Private stream As NetworkStream

    Public Function getPort(ByVal user As String, ByVal pass As String, ByVal newAccount As Boolean, ByVal rsa As RSA, ByRef aes As AES) As Integer
        'aes is passed byref so the underlying object is altered
        Dim tcpClient As New TcpClient
        Try
            tcpClient.Connect(ServerIP, distributePort)
        Catch e As SocketException
            MessageBox.Show("Server is down, please try again later.", "Server down", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return -1
        End Try

        stream = tcpClient.GetStream

        'Send server public key
        sendPublicKey(rsa)

        Dim keyData(255) As Byte
        'Get symmetric AES key from server, encrypted with RSA
        stream.Read(keyData, 0, keyData.Length)
        'The key and IV are converted into base64 and seperated with a space
        Dim keyIV As String() = rsa.decrypt(keyData).Split(" ")
        aes.provider.Key = Convert.FromBase64String(keyIV(0))
        aes.provider.IV = Convert.FromBase64String(keyIV(1))

        'Authenticate the user
        auth(user, pass, newAccount, aes)
        'A 4 digit integer will always return 16 bytes of data when encrypted with AES-256
        Dim portData(15) As Byte
        'Get port sent by server
        stream.Read(portData, 0, portData.Length)

        stream.Close()
        tcpClient.Close()

        'Gives the server time to set up a listener on the correct port
        Threading.Thread.Sleep(100)
        Dim port As String = aes.decrypt(portData)
        If port = "0000" Then
            'Authentication failed
            Return 0
        ElseIf port = "0001" Then
            'Server at max capacity
            Return 1
        Else
            'Authentication successful
            Return port
        End If
    End Function

    Private Sub auth(ByVal user As String, ByVal pass As String, ByVal newAccount As Boolean, ByVal aes As AES)
        'Send username, password, and whether is a new account or not
        Dim authData As Byte()
        authData = aes.encrypt(user & ":" & pass & ":" & newAccount.ToString)
        stream.Write(authData, 0, authData.Length)
    End Sub

    Private Sub sendPublicKey(ByVal rsa As RSA)
        Dim plainData(414) As Byte
        'Get the public key, and send it in plaintext to the server
        plainData = Text.Encoding.ASCII.GetBytes(rsa.provider.ToXmlString(False))
        stream.Write(plainData, 0, plainData.Length)
    End Sub
End Class

Class RSA
    'Asymmetric encryption class
    Property provider As RSACryptoServiceProvider = New RSACryptoServiceProvider(2048, New CspParameters)

    Public Function encrypt(ByVal plaintext As String) As Byte()
        'Convert plaintext string to a byte array
        Dim plainTextBytes As Byte() = Text.Encoding.ASCII.GetBytes(plaintext)
        Dim encryptedBytes As Byte() = provider.Encrypt(plainTextBytes, True)
        Return encryptedBytes
    End Function

    Public Function decrypt(ByVal encryptedBytes As Byte()) As String
        Dim plainTextBytes As Byte() = provider.Decrypt(encryptedBytes, True)
        Return Text.Encoding.ASCII.GetString(plainTextBytes)
    End Function
End Class

Class AES
    'Maximum plaintext length of 1023 characters for a maximum encrypted output of 1024 bytes
    'Symmetric encryption class, key and iv generated automatically by AesCryptoServiceProvider
    'Create a AES Crypto Service Provider, which does the encryption/decryption
    Property provider As AesCryptoServiceProvider = New AesCryptoServiceProvider
    'ICryptoTransform is an interface which accepts the key and IV 
    Private iCrypt As ICryptoTransform
    'Cryptographic functions output to a stream
    Private memoryStream As MemoryStream
    Private cryptStream As CryptoStream

    Public Function encrypt(ByVal plainText As String) As Byte()
        'Convert plaintext string to a byte array
        Dim plainTextBytes As Byte() = Text.Encoding.ASCII.GetBytes(plainText)
        'ICryptoTransform is an interface which accepts the key and IV
        iCrypt = provider.CreateEncryptor(provider.Key, provider.IV)
        'As data passes through cryptStream it is encrypted and written to memoryStream
        memoryStream = New MemoryStream
        cryptStream = New CryptoStream(memoryStream, iCrypt, CryptoStreamMode.Write)
        cryptStream.Write(plainTextBytes, 0, plainTextBytes.Length)
        'FlushFinalBlock ensures the final block is written to memoryStream
        cryptStream.FlushFinalBlock()
        'Set stream position to 0 before begin reading it out
        memoryStream.Position = 0
        'Byte array to hold the encrypted data
        Dim encryptedBytes(memoryStream.Length - 1) As Byte
        'Read the encrypted data out of memoryStream into encryptedBytes
        memoryStream.Read(encryptedBytes, 0, memoryStream.Length)
        cryptStream.Close()
        Return encryptedBytes
    End Function

    Public Function decrypt(ByVal encryptedBytes As Byte()) As String
        'ICryptoTransform is an interface which accepts our key and IV
        iCrypt = provider.CreateDecryptor(provider.Key, provider.IV)
        'As data passes through cryptStream is it decrypted and written to memoryStream
        memoryStream = New MemoryStream
        cryptStream = New CryptoStream(memoryStream, iCrypt, CryptoStreamMode.Write)
        cryptStream.Write(encryptedBytes, 0, encryptedBytes.Length)
        'FlushFinalBlock ensures the final block is writeen to memoryStream
        cryptStream.FlushFinalBlock()
        'Set stream position to 0 before begin reading it out
        memoryStream.Position = 0
        Dim decryptedBytes(memoryStream.Length - 1) As Byte
        'Read the decrypted data out of memoryStream into decryptedBytes
        memoryStream.Read(decryptedBytes, 0, memoryStream.Length)
        cryptStream.Close()
        Return Text.Encoding.ASCII.GetString(decryptedBytes)
    End Function
End Class