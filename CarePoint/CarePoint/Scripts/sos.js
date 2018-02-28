$(document).ready(function () {
    popoverContent = `<div id='idiv-sos-pop'>
                        <form>
                            <textarea class='form-control input-lg' placeholder="What's wrong ?!" rows=10 cols=25 style='resize: none;'></textarea>
                            <input type='submit' value='Send' class='btn btn-danger' style='width: 100%; margin-top: 1em;'>
                        </form>
                    </div>`;
    $('#ibtn-sos-pop').popover({
        html: true,
        container: 'body',
        content: popoverContent
    });
});