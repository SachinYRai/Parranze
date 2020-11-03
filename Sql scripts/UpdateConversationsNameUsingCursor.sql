DECLARE 
    @convo_id uniqueidentifier,
	@val Varchar(Max)

Declare convo_cursor CURSOR
for select Id from Conversations

Open convo_cursor

FETCH NEXT FROM convo_cursor into
@convo_id

while @@FETCH_STATUS =0
begin
 
Select @val = COALESCE(@val + ', ' + UserName, UserName) 
        From AspNetUsers
	where Id in ( select UserId from ConversationUsers where ConversationId = @convo_id)
	print (@val)
	print (@convo_id)

	update Conversations
	set Name = @val 
	where id = @convo_id

	FETCH NEXT FROM convo_cursor into
@convo_id

end;

close convo_cursor;
deallocate convo_cursor;

select * from Conversations

