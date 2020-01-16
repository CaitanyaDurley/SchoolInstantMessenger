<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class findFriendsForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.friendSearchEntry = New System.Windows.Forms.TextBox()
        Me.friendSearchBtn = New System.Windows.Forms.Button()
        Me.findFriendList = New System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        'friendSearchEntry
        '
        Me.friendSearchEntry.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.friendSearchEntry.Location = New System.Drawing.Point(12, 13)
        Me.friendSearchEntry.Name = "friendSearchEntry"
        Me.friendSearchEntry.Size = New System.Drawing.Size(363, 21)
        Me.friendSearchEntry.TabIndex = 0
        Me.friendSearchEntry.Text = "Search for a friend"
        '
        'friendSearchBtn
        '
        Me.friendSearchBtn.Location = New System.Drawing.Point(382, 10)
        Me.friendSearchBtn.Name = "friendSearchBtn"
        Me.friendSearchBtn.Size = New System.Drawing.Size(75, 23)
        Me.friendSearchBtn.TabIndex = 1
        Me.friendSearchBtn.Text = "Search"
        Me.friendSearchBtn.UseVisualStyleBackColor = True
        '
        'findFriendList
        '
        Me.findFriendList.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.findFriendList.FormattingEnabled = True
        Me.findFriendList.ItemHeight = 16
        Me.findFriendList.Location = New System.Drawing.Point(12, 41)
        Me.findFriendList.Name = "findFriendList"
        Me.findFriendList.Size = New System.Drawing.Size(444, 324)
        Me.findFriendList.TabIndex = 2
        '
        'findFriendsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(474, 380)
        Me.Controls.Add(Me.findFriendList)
        Me.Controls.Add(Me.friendSearchBtn)
        Me.Controls.Add(Me.friendSearchEntry)
        Me.Name = "findFriendsForm"
        Me.Text = "findFriendsForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents friendSearchEntry As TextBox
    Friend WithEvents friendSearchBtn As Button
    Friend WithEvents findFriendList As ListBox
End Class
