(function () {
    function getCellValue(row, columnIndex) {
        const cell = row.children[columnIndex];
        if (!cell) return { type: 'string', value: '' };

        if (cell.dataset.sort !== undefined) {
            return { type: 'number', value: parseFloat(cell.dataset.sort) };
        }

        const text = cell.textContent.trim();
        if (text !== '' && /^-?\d+(\.\d+)?$/.test(text)) {
            return { type: 'number', value: parseFloat(text) };
        }

        return { type: 'string', value: text.toLowerCase() };
    }

    function sortTable(table, columnIndex, ascending) {
        const tbody = table.tBodies[0];
        if (!tbody) return;

        const rows = Array.from(tbody.rows);

        rows.sort((rowA, rowB) => {
            const a = getCellValue(rowA, columnIndex);
            const b = getCellValue(rowB, columnIndex);

            const comparison = a.type === 'number' && b.type === 'number'
                ? a.value - b.value
                : String(a.value).localeCompare(String(b.value));

            return ascending ? comparison : -comparison;
        });

        rows.forEach(row => tbody.appendChild(row));
    }

    function initSortableTable(table) {
        const headerRow = table.tHead && table.tHead.rows[0];
        if (!headerRow) return;

        Array.from(headerRow.cells).forEach((th, columnIndex) => {
            if (th.textContent.trim() === '' || th.classList.contains('no-sort')) return;

            th.classList.add('sortable-column');
            th.setAttribute('role', 'button');
            th.setAttribute('tabindex', '0');

            const activate = () => {
                const ascending = th.dataset.sortDirection !== 'asc';

                Array.from(headerRow.cells).forEach(otherTh => {
                    delete otherTh.dataset.sortDirection;
                    otherTh.classList.remove('sort-asc', 'sort-desc');
                });

                th.dataset.sortDirection = ascending ? 'asc' : 'desc';
                th.classList.add(ascending ? 'sort-asc' : 'sort-desc');

                sortTable(table, columnIndex, ascending);
            };

            th.addEventListener('click', activate);
            th.addEventListener('keydown', (event) => {
                if (event.key === 'Enter' || event.key === ' ') {
                    event.preventDefault();
                    activate();
                }
            });
        });
    }

    document.querySelectorAll('table.sortable-table').forEach(initSortableTable);
})();
