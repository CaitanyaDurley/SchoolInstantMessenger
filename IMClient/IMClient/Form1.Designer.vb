<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.msgEntry = New System.Windows.Forms.TextBox()
        Me.Submit = New System.Windows.Forms.Button()
        Me.logIn = New System.Windows.Forms.Button()
        Me.loggedLbl = New System.Windows.Forms.Label()
        Me.userEntry = New System.Windows.Forms.TextBox()
        Me.userLbl = New System.Windows.Forms.Label()
        Me.passEntry = New System.Windows.Forms.TextBox()
        Me.passLbl = New System.Windows.Forms.Label()
        Me.Title = New System.Windows.Forms.Label()
        Me.listView = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.newAccount = New System.Windows.Forms.CheckBox()
        Me.friendsLbl = New System.Windows.Forms.Label()
        Me.friendList = New System.Windows.Forms.ListBox()
        Me.usernameLbl = New System.Windows.Forms.Label()
        Me.friendListBtn = New System.Windows.Forms.Button()
        Me.onlineList = New System.Windows.Forms.ListView()
        Me.findFriendsBtn = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'msgEntry
        '
        Me.msgEntry.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.msgEntry.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.msgEntry.Location = New System.Drawing.Point(209, 622)
        Me.msgEntry.MaxLength = 980
        Me.msgEntry.Multiline = True
        Me.msgEntry.Name = "msgEntry"
        Me.msgEntry.Size = New System.Drawing.Size(768, 42)
        Me.msgEntry.TabIndex = 0
        Me.msgEntry.Text = "Write your message here..."
        '
        'Submit
        '
        Me.Submit.Location = New System.Drawing.Point(983, 631)
        Me.Submit.Name = "Submit"
        Me.Submit.Size = New System.Drawing.Size(75, 23)
        Me.Submit.TabIndex = 2
        Me.Submit.Text = "Submit"
        Me.Submit.UseVisualStyleBackColor = True
        '
        'logIn
        '
        Me.logIn.Location = New System.Drawing.Point(978, 41)
        Me.logIn.Name = "logIn"
        Me.logIn.Size = New System.Drawing.Size(66, 23)
        Me.logIn.TabIndex = 4
        Me.logIn.Text = "Log In"
        Me.logIn.UseVisualStyleBackColor = True
        '
        'loggedLbl
        '
        Me.loggedLbl.AutoSize = True
        Me.loggedLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.loggedLbl.Location = New System.Drawing.Point(978, 13)
        Me.loggedLbl.Name = "loggedLbl"
        Me.loggedLbl.Size = New System.Drawing.Size(69, 15)
        Me.loggedLbl.TabIndex = 5
        Me.loggedLbl.Text = "Logged out"
        '
        'userEntry
        '
        Me.userEntry.Location = New System.Drawing.Point(791, 13)
        Me.userEntry.MaxLength = 25
        Me.userEntry.Name = "userEntry"
        Me.userEntry.Size = New System.Drawing.Size(181, 20)
        Me.userEntry.TabIndex = 6
        '
        'userLbl
        '
        Me.userLbl.AutoSize = True
        Me.userLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.userLbl.Location = New System.Drawing.Point(717, 13)
        Me.userLbl.Name = "userLbl"
        Me.userLbl.Size = New System.Drawing.Size(68, 15)
        Me.userLbl.TabIndex = 7
        Me.userLbl.Text = "Username:"
        '
        'passEntry
        '
        Me.passEntry.Location = New System.Drawing.Point(791, 44)
        Me.passEntry.MaxLength = 991
        Me.passEntry.Name = "passEntry"
        Me.passEntry.Size = New System.Drawing.Size(181, 20)
        Me.passEntry.TabIndex = 8
        Me.passEntry.UseSystemPasswordChar = True
        '
        'passLbl
        '
        Me.passLbl.AutoSize = True
        Me.passLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.passLbl.Location = New System.Drawing.Point(721, 44)
        Me.passLbl.Name = "passLbl"
        Me.passLbl.Size = New System.Drawing.Size(64, 15)
        Me.passLbl.TabIndex = 9
        Me.passLbl.Text = "Password:"
        '
        'Title
        '
        Me.Title.AutoSize = True
        Me.Title.Font = New System.Drawing.Font("Edwardian Script ITC", 50.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Title.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Title.Location = New System.Drawing.Point(241, 1)
        Me.Title.Name = "Title"
        Me.Title.Size = New System.Drawing.Size(423, 79)
        Me.Title.TabIndex = 11
        Me.Title.Text = "Instant Messenger"
        '
        'listView
        '
        Me.listView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listView.CausesValidation = False
        Me.listView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.listView.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.listView.ForeColor = System.Drawing.Color.Navy
        Me.listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.listView.Location = New System.Drawing.Point(209, 88)
        Me.listView.MultiSelect = False
        Me.listView.Name = "listView"
        Me.listView.ShowGroups = False
        Me.listView.Size = New System.Drawing.Size(847, 528)
        Me.listView.TabIndex = 0
        Me.listView.UseCompatibleStateImageBehavior = False
        Me.listView.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = ""
        Me.ColumnHeader1.Width = 830
        '
        'newAccount
        '
        Me.newAccount.AutoSize = True
        Me.newAccount.Location = New System.Drawing.Point(791, 70)
        Me.newAccount.Name = "newAccount"
        Me.newAccount.Size = New System.Drawing.Size(122, 17)
        Me.newAccount.TabIndex = 13
        Me.newAccount.Text = "Create new account"
        Me.newAccount.UseVisualStyleBackColor = True
        '
        'friendsLbl
        '
        Me.friendsLbl.AutoSize = True
        Me.friendsLbl.Font = New System.Drawing.Font("Monotype Corsiva", 30.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.friendsLbl.Location = New System.Drawing.Point(-4, 88)
        Me.friendsLbl.Name = "friendsLbl"
        Me.friendsLbl.Size = New System.Drawing.Size(130, 49)
        Me.friendsLbl.TabIndex = 14
        Me.friendsLbl.Text = "Friends"
        '
        'friendList
        '
        Me.friendList.BackColor = System.Drawing.SystemColors.Window
        Me.friendList.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.friendList.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.friendList.FormattingEnabled = True
        Me.friendList.ItemHeight = 16
        Me.friendList.Location = New System.Drawing.Point(5, 142)
        Me.friendList.Name = "friendList"
        Me.friendList.Size = New System.Drawing.Size(179, 512)
        Me.friendList.TabIndex = 15
        '
        'usernameLbl
        '
        Me.usernameLbl.AutoSize = True
        Me.usernameLbl.Font = New System.Drawing.Font("Monotype Corsiva", 15.75!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.usernameLbl.Location = New System.Drawing.Point(0, 13)
        Me.usernameLbl.Name = "usernameLbl"
        Me.usernameLbl.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.usernameLbl.Size = New System.Drawing.Size(0, 25)
        Me.usernameLbl.TabIndex = 16
        '
        'friendListBtn
        '
        Me.friendListBtn.Location = New System.Drawing.Point(123, 99)
        Me.friendListBtn.Name = "friendListBtn"
        Me.friendListBtn.Size = New System.Drawing.Size(80, 37)
        Me.friendListBtn.TabIndex = 17
        Me.friendListBtn.Text = "Refresh"
        Me.friendListBtn.UseVisualStyleBackColor = True
        '
        'onlineList
        '
        Me.onlineList.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.onlineList.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.onlineList.Location = New System.Drawing.Point(183, 142)
        Me.onlineList.Name = "onlineList"
        Me.onlineList.Size = New System.Drawing.Size(20, 512)
        Me.onlineList.TabIndex = 18
        Me.onlineList.UseCompatibleStateImageBehavior = False
        Me.onlineList.View = System.Windows.Forms.View.List
        '
        'findFriendsBtn
        '
        Me.findFriendsBtn.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.findFriendsBtn.Location = New System.Drawing.Point(5, 70)
        Me.findFriendsBtn.Name = "findFriendsBtn"
        Me.findFriendsBtn.Size = New System.Drawing.Size(198, 23)
        Me.findFriendsBtn.TabIndex = 19
        Me.findFriendsBtn.Text = "Find friends"
        Me.findFriendsBtn.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.findFriendsBtn.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AcceptButton = Me.Submit
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1068, 676)
        Me.Controls.Add(Me.findFriendsBtn)
        Me.Controls.Add(Me.onlineList)
        Me.Controls.Add(Me.friendListBtn)
        Me.Controls.Add(Me.usernameLbl)
        Me.Controls.Add(Me.friendList)
        Me.Controls.Add(Me.friendsLbl)
        Me.Controls.Add(Me.newAccount)
        Me.Controls.Add(Me.listView)
        Me.Controls.Add(Me.Title)
        Me.Controls.Add(Me.passLbl)
        Me.Controls.Add(Me.passEntry)
        Me.Controls.Add(Me.userLbl)
        Me.Controls.Add(Me.userEntry)
        Me.Controls.Add(Me.loggedLbl)
        Me.Controls.Add(Me.logIn)
        Me.Controls.Add(Me.Submit)
        Me.Controls.Add(Me.msgEntry)
        Me.Name = "Form1"
        Me.Opacity = 0.99R
        Me.Text = "Instant Messenger"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents msgEntry As TextBox
    Friend WithEvents Submit As Button
    Friend WithEvents logIn As Button
    Friend WithEvents loggedLbl As Label
    Friend WithEvents userEntry As TextBox
    Friend WithEvents userLbl As Label
    Friend WithEvents passEntry As TextBox
    Friend WithEvents passLbl As Label
    Friend WithEvents Title As Label
    Friend WithEvents newAccount As CheckBox
    Friend WithEvents friendsLbl As Label
    Friend WithEvents friendList As ListBox
    Friend WithEvents usernameLbl As Label
    Friend WithEvents friendListBtn As Button
    Friend WithEvents onlineList As ListView
    Public WithEvents listView As ListView
    Friend WithEvents findFriendsBtn As Button
    Friend WithEvents ColumnHeader1 As ColumnHeader
End Class
