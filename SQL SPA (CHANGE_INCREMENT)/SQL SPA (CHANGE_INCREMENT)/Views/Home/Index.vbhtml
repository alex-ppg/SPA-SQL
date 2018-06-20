<form>
    <div class="form-group">
        <label for="server-name">Server Name</label>
        <input type="text" class="form-control" id="server-name" placeholder="Enter Server Name to connect to">
    </div>
    <div class="form-group">
        <label for="user-id">SQL User ID</label>
        <input type="text" class="form-control" id="user-id" aria-describedby="Enter the SQL connection string on this input box." placeholder="Enter your SQL Server User ID">
    </div>
    <div class="form-group">
        <label for="user-password">SQL Password</label>
        <input type="password" class="form-control" id="user-password" placeholder="Enter your SQL Server User Password">
    </div>
    <div class="form-group">
        <label for="database-name">Database Name</label>
        <input type="text" class="form-control" id="database-name" placeholder="Enter Database to alter">
    </div>
    <div class="form-group">
        <label for="table-name">Table Name</label>
        <input type="text" class="form-control" id="table-name" name="tableName" placeholder="Enter Table to alter">
    </div>
    <div class="form-group">
        <label for="column-name">Column Name</label>
        <input type="text" class="form-control" id="column-name" name="columnName" placeholder="Enter Column to alter">
    </div>
    <div class="form-group">
        <label for="increment-value">New Incrementation Value (Optional)</label>
        <input type="number" class="form-control" id="increment-value" name="incrementValue" placeholder="Enter New Value">
    </div>
    <input type="hidden" name="connectionURL" id="connection-url">
    <script>
        const submitQuery = () => {
            // Extract parameters from above inputs
            const params = {
                serverName: document.getElementById('server-name').value,
                databaseName: document.getElementById('database-name').value,
                userID: document.getElementById('user-id').value,
                password: document.getElementById('user-password').value
            }
            document.getElementById('connection-url').value =
                "Data Source=" +
                params.serverName +
                ";Initial Catalog=" +
                params.databaseName +
                ";Integrated Security=False;User Id=" +
                params.userID +
                ";Password=" +
                params.password +
                ";MultipleActiveResultSets=True"
                ;
        }
    </script>
    <button type="submit" class="btn btn-primary" onclick="submitQuery()">Submit</button>
</form>

@If ViewBag.Title = "Malformed Input" Then
    ViewBag.Title = "Query Creation"

    @<button type="button" Class="btn btn-danger" style="margin-top: 1vmax;pointer-events: none">Error: Malformed Input</button>
End If
