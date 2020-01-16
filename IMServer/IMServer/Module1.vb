Imports System.Net
Imports System.Net.Sockets
Imports System.IO
Imports System.Security.Cryptography

Module Module1
    'How many of a user's friends have to have a common friend for that friend to be recommended to the user
    Const rcmdFriendsDepth As Integer = 2
    Const databasePath As String = "C:\Users\Caitanya\Documents\Visual Studio 2017\Projects\IMServer\IMServer\Users.mdf"
    'Username of admin
    Const adminUsername As String = "Mr Deane"
    'Maximum number of connections allowed
    Const maxConnections As Integer = 2
    'Distribute port is the default port to listen on
    Const distributePort As Integer = 1024
    'Available ports for connections, they are removed and added as ports are used/freed
    Dim ports As New Queue(Of Integer)
    'Holds all the connections with key being the username
    Dim connectionDict As New Dictionary(Of String, instances)

    Class instances
        Property transmission As transmission

        Public Sub initialise(ByVal port As Integer, ByVal user As String, ByVal deviceIPAdress As String, ByVal aes As AES)
            transmission = New transmission(port, deviceIPAdress, aes)
            transmission.communicate(user)
            'When the client disconnects, transmission returns to here
            'Disconnect tcpListener and close socket
            transmission.logOffSub()
        End Sub
    End Class

    Class distAndAuth
        Private socket As Socket
        Private tcpListener As TcpListener
        Private port As Integer
        Private user As String
        Private databaseIO As New databaseIO

        Public Function getPort(ByVal rsa As RSA, ByRef aes As AES) As String()
            'Accept any connection on distribute port
            tcpListener = New TcpListener(IPAddress.Parse("0.0.0.0"), distributePort)
            'Waiting for a connection here
            tcpListener.Start()

            'Accepts incoming connection
            socket = tcpListener.AcceptSocket
            'Get user's IP adress
            Dim deviceIPAdress As String = tcpListener.LocalEndpoint.ToString.Split(":")(0)

            'Get public key from client
            getPublicKey(rsa)
            'Send symmetric key to client, encrypted using rsa
            socket.Send(rsa.encrypt(Convert.ToBase64String(aes.provider.Key) & " " & Convert.ToBase64String(aes.provider.IV)))

            'Checks client's login details and gets username
            user = auth(aes)

            If user = "NULL" Then
                'User is sent 0000 since they are not authorised. Client is expecting 4 digits hence 4 zeroes
                socket.Send(aes.encrypt("0000"))
                port = 0
                deviceIPAdress = "NULL"
            Else
                If ports.Count > 0 Then
                    'Sends details of an open port to the client
                    port = ports.First
                    ports.Dequeue()
                    socket.Send(aes.encrypt(port))
                Else
                    'User is sent 0001 to signify the server has reached its maximum number of connections
                    port = 0
                    socket.Send(aes.encrypt("0001"))
                    deviceIPAdress = "NULL"
                End If
            End If

            socket.Close()
            tcpListener.Stop()
            'If the user is not authenticated or there are no free ports return {0, "NULL"}
            Return {port.ToString, user, deviceIPAdress}
        End Function

        Private Function auth(ByVal aes As AES) As String
            Dim datas As String
            Dim tmpBytes(1023) As Byte
            Dim length As Integer

            'Receive details in format "user:password:newAccount" where newAccount is a boolean value
            length = socket.Receive(tmpBytes)

            'Array of correct size with only data received created
            Dim authData(length - 1) As Byte
            Array.Copy(tmpBytes, authData, length)

            'Convert into plaintext
            datas = aes.decrypt(authData)

            If datas.EndsWith(":True") Then
                'New user so create new account
                datas = datas.Replace(":True", "")
                'Write details to database
                'Return username if okay, or "NULl" if username already taken
                Return databaseIO.newUser(datas)
            Else
                'Existing user, check details, return username or "NULL" if incorrect details 
                datas = datas.Replace(":False", "")
                Return databaseIO.existingUser(datas)
            End If
        End Function

        Private Sub getPublicKey(ByRef rsa As RSA)
            Dim plainData(414) As Byte
            Dim publicKey As String

            'Receive public key
            socket.Receive(plainData)
            publicKey = Text.Encoding.ASCII.GetString(plainData)

            'Give rsa the public key
            rsa.provider.FromXmlString(publicKey)
        End Sub
    End Class

    Class transmission
        Property statusCode As Integer = 0
        Property socket As Socket
        Private tcpListener As TcpListener
        Property aes As AES
        Private databaseIO As New databaseIO
        'The following are identifiers sent from the client to the server and vice versa in order to identify the command
        Private Const adminString As String = "!$%IM_ADMIN^&*"
        Private Const logOffString As String = "!$%IM_LOGOFF^&*"
        Private Const partnerString As String = "!$%IM_PARTNER^&*"
        Private Const messageString As String = "!$%IM_MESSAGE^&*"
        Private Const logOffForceString As String = "!$%IM_LOGOFFFORCE^&*"
        Private Const friendsPopString As String = "!$%IM_FRIENDSPOPULATE^&*"
        Private Const searchFriendsString As String = "!$%IM_SEARCHFRIENDS^&*"
        Private Const friendRequestString As String = "!$%IM_FRIENDREQUEST^&*"
        Private Const recommendFriendsString As String = "!$%IM_RCMDFRIENDS^&*"
        'The following are also commands, but specific to admin
        Private Const adminGetUsersString As String = "!$%IM_GETUSERS^&*"
        Private Const adminDeleteUserString As String = "!$%IM_DELUSER^&*"
        Private Const adminGetMessageString As String = "!$%IM_GETMESSAGE^&*"
        Private Const adminChangePasswordString As String = "!$%IM_CHNGPSSWD^&*"
        Private Const adminGetFriendRequestString As String = "!$%IM_GETFRNDRQST^&*"
        Private Const adminAcceptFriendRequestString As String = "!$%IM_ACPTFRNDRQST^&*"
        Private Const adminRejectFriendRequestString As String = "!$%IM_RJCTFRNDRQST^&*"

        Public Sub New(ByVal port As Integer, ByVal deviceIPAdress As String, ByVal aesp As AES)
            aes = aesp

            'Accept any connection on port specified from sub Main() and instances
            tcpListener = New TcpListener(IPAddress.Parse("0.0.0.0"), port)
            'Waiting for a connection here
            tcpListener.Start()

            'Ensure the device connecting here is the one authorised by distAndAuth
            If tcpListener.LocalEndpoint.ToString.Split(":")(0) = deviceIPAdress Then
                'Accepts incoming connection
                socket = tcpListener.AcceptSocket
            Else
                tcpListener.Stop()
                statusCode = 1
            End If
        End Sub

        Public Sub logOffSub()
            'Closes all connections and stops listening for new connections
            'Disconnect tcpListener and close socket
            If statusCode <> 1 Then
                If statusCode = 2 Then
                    'Indicates that a second device is trying to connect on the same account, so disconnect the first
                    socket.Send(aes.encrypt(logOffForceString))
                End If
                socket.Close()
                tcpListener.Stop()
            End If
            statusCode = 1
        End Sub

        Private Sub userOnlineRead(ByVal user As String, ByVal partner As String)
            'Gets messages between two users from userOnlineRead, then formats into suitable lengths for sending
            Dim data As String = ""
            For Each i In databaseIO.userOnlineRead(user, partner)
                If data.Length + i.Length <= 1023 - partnerString.Length Then
                    data += partnerString & i
                Else
                    'Send what will fit in a 1024 byte array
                    socket.Send(aes.encrypt(data))
                    'Start new string
                    data = partnerString & i
                    'Wait for client to read out and display the previous string
                    Threading.Thread.Sleep(20)
                End If
            Next
            'If last iteration didn't go over 1023 - partnerString.Length, then there is still data waiting to be sent
            socket.Send(aes.encrypt(data))
        End Sub

        Public Sub communicate(ByVal user As String)
            Dim data As String
            Dim partner As String
            Dim tmpBytes(1023) As Byte
            Dim length As Integer
            Dim admin As Boolean = False

            Do
                Try
                    'Transfers the bytes transmitted from client into a byte array and gets length of data received
                    length = socket.Receive(tmpBytes)
                Catch
                    'Most likely client disconnected forcefully
                    Exit Do
                End Try

                'Array of correct size only with data received
                Dim recvBytes(length - 1) As Byte
                Array.Copy(tmpBytes, recvBytes, length)

                'Converts the bytes into ASCII
                data = aes.decrypt(recvBytes)

                'Single quotations are an escape character in sql
                data = data.Replace("'", "''")

                If data = logOffString Then
                    'This is the message sent when the client clicks log off
                    Exit Do
                ElseIf data.StartsWith(partnerString) Then
                    'Change partner
                    partner = data.Substring(partnerString.Length)
                    'Send all messages to client
                    userOnlineRead(user, partner)
                ElseIf data = friendsPopString Then
                    'Get all the friends of this user
                    data += databaseIO.friendsPopulate(user, 0, True)
                    'Send string of users to client
                    socket.Send(aes.encrypt(data))
                    Threading.Thread.Sleep(20)
                    'If the user is the admin, let the client know. Put here because first action by client is friendsPopulate
                    If user = adminUsername Then
                        socket.Send(aes.encrypt(adminString))
                        admin = True
                    End If
                ElseIf data.StartsWith(searchFriendsString) Then
                    'Return a list of all usernames like the one provided
                    socket.Send(aes.encrypt(searchFriendsString & databaseIO.searchFriends(data.Substring(searchFriendsString.Length))))
                ElseIf data.StartsWith(friendRequestString) Then
                    'Send a friend request
                    databaseIO.friendRequest(user, data.Substring(friendRequestString.Length))
                ElseIf data.StartsWith(messageString) Then
                    'Send message to partner if they're online
                    If connectionDict.ContainsKey(partner) Then
                        connectionDict.Item(partner).transmission.socket.Send(connectionDict.Item(partner).transmission.aes.encrypt(data))
                    End If
                    'Store message for user and partner
                    data = data.Substring(data.IndexOf(":") + 2)
                    databaseIO.userOfflineWrite(user, partner, data)
                ElseIf data = recommendFriendsString Then
                    'Recommend the user some friends
                    data = searchFriendsString & databaseIO.recommendFriends(user)
                    socket.Send(aes.encrypt(data))
                End If
                'All admin functionaility available here, so even if a user tricked the client into showing the admin form, it would be useless
                If admin = True Then
                    If data.StartsWith(adminGetMessageString) Then
                        'Get messages between two users using existing functionality
                        data = data.Substring(adminGetMessageString.Length)
                        userOnlineRead(data.Split(",")(0), data.Split(",")(1))
                    ElseIf data = adminGetUsersString Then
                        'Make a search friends request using the Like operator's equivalent of a wildcard
                        socket.Send(aes.encrypt(searchFriendsString & databaseIO.searchFriends("%")))
                    ElseIf data = adminGetFriendRequestString Then
                        'Get all unapproved friend requests
                        socket.Send(aes.encrypt(friendsPopString & databaseIO.getFriendRequests()))
                    ElseIf data.StartsWith(adminAcceptFriendRequestString) Then
                        data = data.Substring(adminAcceptFriendRequestString.Length)
                        databaseIO.acceptFriendRequest(data.Split(",")(0), data.Split(",")(1))
                    ElseIf data.StartsWith(adminRejectFriendRequestString) Then
                        data = data.Substring(adminRejectFriendRequestString.Length)
                        databaseIO.rejectFriendRequest(data.Split(",")(0), data.Split(",")(1))
                    ElseIf data.StartsWith(adminChangePasswordString) Then
                        data = data.Substring(adminChangePasswordString.Length)
                        Dim tmpSplit As String() = Split(data, ",", 2)
                        databaseIO.changePassword(tmpSplit(0), tmpSplit(1))
                    ElseIf data.StartsWith(adminDeleteUserString) Then
                        databaseIO.deleteUser(data.Substring(adminDeleteUserString.Length))
                    End If
                End If
            Loop
        End Sub
    End Class

    Class databaseIO
        Private sqlCon As SqlClient.SqlConnection
        Private sqlCmd As SqlClient.SqlCommand
        Private sqlResult As SqlClient.SqlDataReader
        Private sha As SHA

        Public Sub New()
            sqlCon = New SqlClient.SqlConnection
            sqlCmd = New SqlClient.SqlCommand
            sqlCon.ConnectionString = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" & databasePath
            sqlCmd.Connection = sqlCon
            'Parameters needed because i can't string.format in a byte array
            sqlCmd.Parameters.Add("@passwordHash", SqlDbType.Binary)
            sqlCmd.Parameters.Add("@salt", SqlDbType.Binary)
            sqlCmd.Parameters("@passwordHash").Value = DBNull.Value
            sqlCmd.Parameters("@salt").Value = DBNull.Value
            sha = New SHA
        End Sub

        Private Function getUserID(ByVal user As String) As Integer
            sqlCon.Open()
            'Gets user's ID
            sqlCmd.CommandText = String.Format("SELECT tblUsers.UserID FROM tblUsers WHERE tblUsers.Username = '{0}'", user)
            sqlResult = sqlCmd.ExecuteReader
            sqlResult.Read()
            Dim UserID As Integer = sqlResult(0)
            sqlResult.Close()
            sqlCon.Close()
            Return UserID
        End Function

        Public Function recommendFriends(ByVal user As String) As String
            Dim userID As Integer = getUserID(user)
            sqlCon.Open()
            'Get all friends of the current user that don't include the admin and that are accepted
            sqlCmd.CommandText = String.Format("SELECT UserID, FriendID FROM tblFriends WHERE Accepted = 'True' AND (UserID = {0} OR FriendID = {0}) AND NOT UserID = 1", userID)
            sqlResult = sqlCmd.ExecuteReader
            Dim data As New List(Of Integer)
            While sqlResult.Read
                'Ensure the FriendID is obtained not the UserID
                If sqlResult(0) = userID Then
                    data.Add(sqlResult(1))
                Else
                    data.Add(sqlResult(0))
                End If
            End While
            sqlCon.Close()
            Dim nameString As String = ""
            For Each i In data
                'Get all the friends of each friend of the user in turn
                nameString += friendsPopulate("", i, False)
            Next
            'Remove occurences of the original user
            nameString = nameString.Replace("," & user, "")
            Dim arraySplit As String() = nameString.Split(",")
            Dim nameStringList As New List(Of String)
            'Iterate through the friends, if they appear more than or equal to rcmdFriendsDepth times in the array, then add to the list
            arraySplit.ToList.ForEach(Sub(username) If arraySplit.Count(Function(s) s = username) >= rcmdFriendsDepth Then nameStringList.Add(username))
            'Since this operation will go through users more than once, remove duplicates before concatenating into a string (if i called .Distinct first it couldn't count the number of users)
            Return String.Join(",", nameStringList.Distinct)
        End Function

        Public Sub deleteUser(ByVal username As String)
            Dim userID As Integer = getUserID(username)
            sqlCon.Open()
            'Delete all the user's messages, friend requests and finaly the user record itself 
            sqlCmd.CommandText = String.Format("DELETE FROM tblMessages WHERE UserID = {0} OR FriendID = {0}; DELETE FROM tblFriends WHERE UserID = {0} OR FriendID = {0}; DELETE FROM tblUsers WHERE UserID = {0}", userID)
            sqlCmd.ExecuteReader()
            sqlCon.Close()
        End Sub

        Public Sub changePassword(ByVal user As String, ByVal password As String)
            sqlCon.Open()
            'Get hash and used salt of user's new password
            Dim hashSaltSplit As Byte()() = sha.hash(password, Nothing)
            sqlCmd.Parameters("@passwordHash").Value = hashSaltSplit(0)
            sqlCmd.Parameters("@salt").Value = hashSaltSplit(1)
            sqlCmd.CommandText = String.Format("UPDATE tblUsers SET PasswordHash = @passwordHash, Salt = @salt WHERE Username = '{0}'", user)
            sqlCmd.ExecuteReader()
            sqlCon.Close()
        End Sub

        Public Sub acceptFriendRequest(ByVal userName As String, ByVal friendName As String)
            Dim userID As Integer = getUserID(userName)
            Dim friendID As Integer = getUserID(friendName)
            sqlCon.Open()
            sqlCmd.CommandText = String.Format("UPDATE tblFriends SET Accepted = 'True' WHERE (UserID = {0} AND FriendID = {1}) OR (FriendID = {0} AND UserID = {1})", userID, friendID)
            sqlCmd.ExecuteReader()
            sqlCon.Close()
        End Sub

        Public Sub rejectFriendRequest(ByVal userName As String, ByVal friendName As String)
            Dim userID As Integer = getUserID(userName)
            Dim friendID As Integer = getUserID(friendName)
            sqlCon.Open()
            sqlCmd.CommandText = String.Format("DELETE FROM tblFriends WHERE (UserID = {0} AND FriendID = {1}) OR (FriendID = {0} AND UserID = {1})", userID, friendID)
            sqlCmd.ExecuteReader()
            sqlCon.Close()
        End Sub

        Public Function getFriendRequests() As String
            sqlCon.Open()
            'Get unapproved friend requests
            sqlCmd.CommandText = "SELECT tblUsers.Username FROM tblUsers, tblFriends WHERE tblFriends.Accepted = 'False' AND (tblFriends.UserID = tblUsers.UserID OR tblFriends.FriendID = tblUsers.UserID)"
            sqlResult = sqlCmd.ExecuteReader
            Dim friendRequestString As String = ""
            If sqlResult.HasRows Then
                While sqlResult.Read()
                    friendRequestString += "," & sqlResult(0)
                End While
            Else
                friendRequestString += ",NULL"
            End If
            sqlResult.Close()
            sqlCon.Close()
            Return friendRequestString
        End Function

        Public Sub friendRequest(ByVal user As String, ByVal partnerToBe As String)
            Dim userID As Integer = getUserID(user)
            Dim partnerToBeID As Integer = getUserID(partnerToBe)
            sqlCon.Open()
            sqlCmd.CommandText = String.Format("INSERT INTO tblFriends VALUES ('{0}', '{1}', 'False')", userID, partnerToBeID)
            sqlCmd.ExecuteReader()
            sqlCon.Close()
        End Sub

        Public Function searchFriends(ByVal data As String) As String
            sqlCon.Open()
            'Get all usernames like the one sent by the user, % is the equivalent of .* in regex
            sqlCmd.CommandText = String.Format("SELECT tblUsers.Username FROM tblUsers WHERE Username LIKE '%{0}%'", data)
            sqlResult = sqlCmd.ExecuteReader
            data = ""
            While sqlResult.Read()
                data += "," & sqlResult(0)
            End While
            sqlResult.Close()
            sqlCon.Close()
            Return data
        End Function

        Public Function friendsPopulate(ByVal user As String, ByVal userID As Integer, ByVal onlineStatus As Boolean) As String
            'Allows this function to be called if you know either the username or the UserID
            If userID = 0 Then
                userID = getUserID(user)
            End If
            sqlCon.Open()
            'Gets the usernames of the user's friends
            sqlCmd.CommandText = String.Format("SELECT tblUsers.Username FROM tblUsers, tblFriends WHERE tblFriends.Accepted = 'True' AND ((tblFriends.UserID = '{0}' AND tblFriends.FriendID = tblUsers.UserID) OR (tblFriends.FriendID = '{0}' AND tblFriends.UserID = tblUsers.UserID))", userID)
            sqlResult = sqlCmd.ExecuteReader
            Dim data As String = ""
            'Depending on whether the caller wants to know the online status of the users
            If onlineStatus = True Then
                'Concat usernames into a string, format username:online where online is boolean
                While sqlResult.Read
                    data += "," & sqlResult(0) & ":" & connectionDict.ContainsKey(sqlResult(0)).ToString()
                End While
            Else
                While sqlResult.Read
                    data += "," & sqlResult(0)
                End While
            End If
            sqlResult.Close()
            sqlCon.Close()
            Return data
        End Function

        Public Function newUser(ByVal data As String) As String
            Dim localUsername As String = data.Split(":")(0)
            sqlCon.Open()
            'Get hash and used salt of user's password
            Dim hashSaltSplit As Byte()() = sha.hash(data.Split(":")(1), Nothing)
            sqlCmd.Parameters("@passwordHash").Value = hashSaltSplit(0)
            sqlCmd.Parameters("@salt").Value = hashSaltSplit(1)
            'If the username exists return a 1, else insert the username, password hash and salt
            sqlCmd.CommandText = String.Format("IF NOT EXISTS (SELECT * FROM tblUsers WHERE Username = '{0}') BEGIN INSERT INTO tblUsers VALUES ('{0}', @passwordHash, @salt) END ELSE BEGIN SELECT 1 END", localUsername)
            sqlResult = sqlCmd.ExecuteReader()
            sqlResult.Read()
            If sqlResult.HasRows Then
                'Username already exists in database; return error message
                sqlResult.Close()
                sqlCon.Close()
                Return "NULL"
            Else
                'User details added, username returned, user auto friended with adminUsername
                sqlResult.Close()
                sqlCon.Close()
                sqlCmd.CommandText = String.Format("INSERT INTO tblFriends VALUES ('1', '{0}', 'True')", getUserID(localUsername))
                sqlCon.Open()
                sqlCmd.ExecuteReader()
                sqlCon.Close()
                Return localUsername
            End If
        End Function

        Public Function existingUser(data As String) As String
            Dim localUsername As String = data.Split(":")(0)
            'Check if user details exist in database
            sqlCon.Open()
            sqlCmd.CommandText = String.Format("SELECT PasswordHash, Salt FROM tblUsers WHERE Username = '{0}'", localUsername)
            sqlResult = sqlCmd.ExecuteReader
            sqlResult.Read()

            'Compare user supplied password with salted hash
            If sqlResult.HasRows AndAlso sha.compare(data.Split(":")(1), sqlResult(1), sqlResult(0)) = True Then
                sqlResult.Close()
                sqlCon.Close()
                'Return username
                Return localUsername
            Else
                sqlResult.Close()
                sqlCon.Close()
                Return "NULL"
            End If
        End Function

        Public Sub userOfflineWrite(ByVal user As String, ByVal partner As String, ByVal data As String)
            Dim UserID As Integer = getUserID(user)
            Dim PartnerID As Integer = getUserID(partner)
            sqlCon.Open()
            sqlCmd.CommandText = String.Format("INSERT INTO tblMessages VALUES ('{0}', '{1}', '{2}')", data, UserID, PartnerID)
            sqlCmd.ExecuteReader()
            sqlCon.Close()
        End Sub

        Public Function userOnlineRead(ByVal user As String, ByVal partner As String) As List(Of String)
            Dim UserID As Integer = getUserID(user)
            Dim PartnerID As Integer = getUserID(partner)
            Dim data As New List(Of String)
            sqlCon.Open()
            'Get messages sent from user to friend or vice versa. As messages are added to database in chronological order, they will be returned as such
            sqlCmd.CommandText = String.Format("SELECT tblMessages.UserID, tblMessages.Text FROM tblMessages WHERE (UserID = '{0}' AND FriendID = '{1}') OR (FriendID = '{0}' AND UserID = '{1}')", UserID, PartnerID)
            sqlResult = sqlCmd.ExecuteReader
            While sqlResult.Read()
                'Let client know who sent the message
                Select Case sqlResult(0)
                    Case UserID
                        data.Add(user & ": " & sqlResult(1))
                    Case PartnerID
                        data.Add(partner & ": " & sqlResult(1))
                End Select
            End While
            sqlCon.Close()
            Return data
        End Function
    End Class

    Class AES
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

    Class SHA
        Private shaManaged As SHA512 = New SHA512Managed
        Private rng As RNGCryptoServiceProvider = New RNGCryptoServiceProvider

        Public Function hash(ByVal password As String, ByVal salt As Byte()) As Byte()()
            'Get the plaintext password in bytes
            Dim passwordBytes As Byte() = Text.Encoding.ASCII.GetBytes(password)
            'Allows the function to be called with or without a known hash
            If salt Is Nothing Then
                ReDim salt(63)
                rng.GetBytes(salt)
            End If
            Dim inputBytes(passwordBytes.Length + salt.Length - 1) As Byte
            'Concatenate the passwordBytes and Salt into a single byte array
            Array.Copy(passwordBytes, 0, inputBytes, 0, passwordBytes.Length)
            Array.Copy(salt, 0, inputBytes, passwordBytes.Length, salt.Length)
            'Hash and return salt in case it wasn't provided
            Return {shaManaged.ComputeHash(inputBytes), salt}
        End Function

        Public Function compare(ByVal password As String, ByVal salt As Byte(), ByVal storedHash As Byte()) As Boolean
            'Hash the provided password with the provided salt and see if the returned hash matches
            If hash(password, salt)(0).SequenceEqual(storedHash) Then
                Return True
            Else
                Return False
            End If
        End Function
    End Class

    Sub Main()
        'Fill ports queue with desired number of ports
        For i = distributePort + 1 To distributePort + maxConnections
            ports.Enqueue(i)
        Next

        Dim dist As New distAndAuth
        Dim port As Integer
        Dim user As String
        Dim deviceIPAdress As String
        Dim portUserSplit(1) As String

        Console.WriteLine("### Instant Messaging Server ###" & vbNewLine)
        Console.WriteLine("Waiting for connections" & vbNewLine)

        Do
            Dim rsa As New RSA
            Dim aes As New AES
            'dist.getPort returns an available port so transmission can be notified of the correct port to start a listener on
            'dist.getPort also returns the username the client authenticated with
            portUserSplit = dist.getPort(rsa, aes)
            'The program hangs here until a connection is made and getPort returns {port, user}
            port = portUserSplit(0)
            user = portUserSplit(1)
            deviceIPAdress = portUserSplit(2)

            If port <> 0 And user <> "NULL" And deviceIPAdress <> "NULL" Then
                'If a user tries to connect from two devices then disconnect the first device
                If connectionDict.ContainsKey(user) Then
                    connectionDict.Item(user).transmission.statusCode = 2
                    connectionDict.Item(user).transmission.logOffSub()
                    Threading.Thread.Sleep(1000)
                End If
                'Add connection to connectionDictionary with key = username authenticated with
                connectionDict.Add(user, New instances)
                'Start new thread for the communication with this client
                newThread(port, user, deviceIPAdress, aes)
                Console.WriteLine(connectionDict.Count & " users connected")
            End If
        Loop
    End Sub

    Private Sub newThread(ByVal port As Integer, ByVal user As String, ByVal deviceIPAdress As String, ByVal aes As AES)
        Dim t As New Threading.Thread(
            Sub()
                connectionDict.Item(user).initialise(port, user, deviceIPAdress, aes)
                'Instance of instances is removed from connectionDict
                connectionDict.Remove(user)
                'The now available port is enqueued
                ports.Enqueue(port)
            End Sub
        )
        t.Start()
    End Sub
End Module