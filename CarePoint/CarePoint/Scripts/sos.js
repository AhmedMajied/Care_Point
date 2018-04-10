$(document).ready(function () {
    popoverContent = `<div id='idiv-sos-pop'>
                        <form>
                            <textarea class='form-control input-lg' placeholder="What's wrong ?!" rows=10 cols=25 style='resize: none;'></textarea>
                            <hr>
                            <label class="text-danger">Send to:</label><br>
                            <span class="cspn-radio-chck">
                                <input id='ichk-hospitals' class='cchck-danger' type='checkbox' checked>
                                <label for='ichk-hospitals'>Hospitals</label>
                            </span>
                            <span class="cspn-radio-chck">
                                <input id='ichk-family' class='cchck-danger' type='checkbox'>
                                <label for='ichk-family'>Family</label>
                            </span>
                            <span class="cspn-radio-chck">
                                <input id='ichk-friends' class='cchck-danger' type='checkbox'>
                                <label for='ichk-friends'>Friends</label>
                            </span>
                            <input type='submit' value='Send' class='btn btn-danger' style='width: 100%; margin-top: 1em;'>
                        </form>
                    </div>`;
    $('#ibtn-sos-pop').popover({
        html: true,
        container: 'body',
        content: popoverContent
    });
});