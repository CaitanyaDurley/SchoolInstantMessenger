<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class adminForm
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.friendRequestListBox = New System.Windows.Forms.ListBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.userListBox = New System.Windows.Forms.ListBox()
        Me.viewMsgBtn = New System.Windows.Forms.Button()
        Me.passChangeEntry = New System.Windows.Forms.TextBox()
        Me.passChangeBtn = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.requestAcceptBtn = New System.Windows.Forms.Button()
        Me.requestRejectBtn = New System.Windows.Forms.Button()
        Me.delUserBtn = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(35, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(149, 24)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Friend Requests"
        '
        'friendRequestListBox
        '
        Me.friendRequestListBox.FormattingEnabled = True
        Me.friendRequestListBox.Location = New System.Drawing.Point(12, 48)
        Me.friendRequestListBox.Name = "friendRequestListBox"
        Me.friendRequestListBox.Size = New System.Drawing.Size(206, 433)
        Me.friendRequestListBox.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(385, 21)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(81, 24)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "User List"
        '
        'userListBox
        '
        Me.userListBox.FormattingEnabled = True
        Me.userListBox.Location = New System.Drawing.Point(351, 48)
        Me.userListBox.Name = "userListBox"
        Me.userListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.userListBox.Size = New System.Drawing.Size(149, 433)
        Me.userListBox.TabIndex = 3
        '
        'viewMsgBtn
        '
        Me.viewMsgBtn.Location = New System.Drawing.Point(506, 104)
        Me.viewMsgBtn.Name = "viewMsgBtn"
        Me.viewMsgBtn.Size = New System.Drawing.Size(125, 21)
        Me.viewMsgBtn.TabIndex = 4
        Me.viewMsgBtn.Text = "View Messages"
        Me.viewMsgBtn.UseVisualStyleBackColor = True
        '
        'passChangeEntry
        '
        Me.passChangeEntry.Location = New System.Drawing.Point(506, 304)
        Me.passChangeEntry.Name = "passChangeEntry"
        Me.passChangeEntry.Size = New System.Drawing.Size(126, 20)
        Me.passChangeEntry.TabIndex = 5
        Me.passChangeEntry.Text = "Enter new password..."
        '
        'passChangeBtn
        '
        Me.passChangeBtn.Location = New System.Drawing.Point(506, 253)
        Me.passChangeBtn.Name = "passChangeBtn"
        Me.passChangeBtn.Size = New System.Drawing.Size(125, 21)
        Me.passChangeBtn.TabIndex = 6
        Me.passChangeBtn.Text = "Change Password"
        Me.passChangeBtn.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(515, 21)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(116, 24)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "User Actions"
        '
        'requestAcceptBtn
        '
        Me.requestAcceptBtn.Location = New System.Drawing.Point(224, 104)
        Me.requestAcceptBtn.Name = "requestAcceptBtn"
        Me.requestAcceptBtn.Size = New System.Drawing.Size(125, 21)
        Me.requestAcceptBtn.TabIndex = 8
        Me.requestAcceptBtn.Text = "Accept"
        Me.requestAcceptBtn.UseVisualStyleBackColor = True
        '
        'requestRejectBtn
        '
        Me.requestRejectBtn.Location = New System.Drawing.Point(224, 178)
        Me.requestRejectBtn.Name = "requestRejectBtn"
        Me.requestRejectBtn.Size = New System.Drawing.Size(125, 21)
        Me.requestRejectBtn.TabIndex = 9
        Me.requestRejectBtn.Text = "Reject"
        Me.requestRejectBtn.UseVisualStyleBackColor = True
        '
        'delUserBtn
        '
        Me.delUserBtn.Location = New System.Drawing.Point(506, 178)
        Me.delUserBtn.Name = "delUserBtn"
        Me.delUserBtn.Size = New System.Drawing.Size(125, 21)
        Me.delUserBtn.TabIndex = 10
        Me.delUserBtn.Text = "Remove user"
        Me.delUserBtn.UseVisualStyleBackColor = True
        '
        'adminForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(647, 490)
        Me.Controls.Add(Me.delUserBtn)
        Me.Controls.Add(Me.requestRejectBtn)
        Me.Controls.Add(Me.requestAcceptBtn)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.passChangeBtn)
        Me.Controls.Add(Me.passChangeEntry)
        Me.Controls.Add(Me.viewMsgBtn)
        Me.Controls.Add(Me.userListBox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.friendRequestListBox)
        Me.Controls.Add(Me.Label1)
        Me.Name = "adminForm"
        Me.Text = "adminForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents friendRequestListBox As ListBox
    Friend WithEvents Label2 As Label
    Friend WithEvents userListBox As ListBox
    Friend WithEvents viewMsgBtn As Button
    Friend WithEvents passChangeEntry As TextBox
    Friend WithEvents passChangeBtn As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents requestAcceptBtn As Button
    Friend WithEvents requestRejectBtn As Button
    Friend WithEvents delUserBtn As Button
End Class
