document.addEventListener('DOMContentLoaded', function() {
    const radioButtons = document.querySelectorAll('input[name="chuc_vu"]');
    const sv = document.querySelectorAll('.chuc_vu-sv');
    const gv = document.querySelectorAll('.chuc_vu-gv');
    
    function handleRadioChange() {
        const selectedValue = document.querySelector('input[name="chuc_vu"]:checked').value;
        
        if (selectedValue === 'sv') {
            sv.forEach(input => input.classList.remove('hidden'));
            gv.forEach(input => input.classList.add('hidden'));
        } else if (selectedValue === 'gv') {
            sv.forEach(input => input.classList.add('hidden'));
            gv.forEach(input => input.classList.remove('hidden'));
        }
    }
    
    radioButtons.forEach(radio => {
        radio.addEventListener('change', handleRadioChange);
    });

    handleRadioChange();
});